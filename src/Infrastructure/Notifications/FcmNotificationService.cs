using Application.Abstractions.Notifications;
using Domain.Notifications;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SharedKernel;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Infrastructure.Notifications;

public class FcmNotificationService : IFcmNotificationService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly ILogger<FcmNotificationService> _logger;

    private static readonly object _lock = new();
    private static string? _cachedAccessToken;
    private static DateTime _tokenExpiry = DateTime.MinValue;

    public FcmNotificationService(
        HttpClient httpClient,
        IConfiguration configuration,
        ILogger<FcmNotificationService> logger)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<Result> SendNotificationAsync(
        string fcmToken,
        string title,
        string body,
        string type,
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(fcmToken))
            {
                return Result.Failure(FcmErrors.TokenNotProvided);
            }

            var accessToken = await GetAccessTokenAsync(cancellationToken);
            if (string.IsNullOrEmpty(accessToken))
            {
                return Result.Failure(FcmErrors.AccessTokenFailed);
            }

            var message = BuildMessage(fcmToken, title, body, type);

            var jsonContent = JsonSerializer.Serialize(
                message,
                new JsonSerializerOptions { WriteIndented = true }
            );

            var projectId = _configuration["Fcm:ProjectId"];
            var url = $"https://fcm.googleapis.com/v1/projects/{projectId}/messages:send";

            using var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(
                    jsonContent,
                    Encoding.UTF8,
                    "application/json"
                )
            };

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", accessToken);

            request.Headers.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json")
            );

            _logger.LogInformation("FCM Request JSON:\n{Json}", jsonContent);

            var response = await _httpClient.SendAsync(request, cancellationToken);

            var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation(
                    "Notificación FCM enviada exitosamente. Token: {TokenMask}",
                    MaskToken(fcmToken)
                );
                return Result.Success();
            }

            var errorMessage = ExtractFcmErrorMessage(responseBody);

            _logger.LogError(
                "Error FCM. Status: {StatusCode}. Body: {Body}",
                response.StatusCode,
                responseBody
            );

            var error = Error.Problem(
                "Fcm.SendFailed",
                $"Error FCM ({response.StatusCode}): {errorMessage}"
            );

            return Result.Failure(error);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al enviar notificación FCM");

            return Result.Failure(
                Error.Problem(
                    "Fcm.Exception",
                    $"Error al enviar notificación: {ex.Message}"
                )
            );
        }
    }

    private object BuildMessage(string token, string title, string body, string type)
    {
        return new
        {
            message = new
            {
                token,
                notification = new
                {
                    title,
                    body
                },
                data = new
                {
                    type
                },
                android = new
                {
                    priority = "HIGH"
                }
            }
        };
    }

    private async Task<string?> GetAccessTokenAsync(CancellationToken cancellationToken)
    {
        lock (_lock)
        {
            if (!string.IsNullOrEmpty(_cachedAccessToken) &&
                _tokenExpiry > DateTime.UtcNow.AddMinutes(5))
            {
                return _cachedAccessToken;
            }
        }

        try
        {
            var projectId = _configuration["Fcm:ProjectId"];
            var privateKey = _configuration["Fcm:PrivateKey"];
            var clientEmail = _configuration["Fcm:ClientEmail"];

            if (string.IsNullOrEmpty(privateKey) ||
                string.IsNullOrEmpty(clientEmail) ||
                string.IsNullOrEmpty(projectId))
            {
                _logger.LogError("Configuración de Firebase incompleta");
                return null;
            }

            var credentialsJson = $$"""
            {
                "type": "service_account",
                "project_id": "{{projectId}}",
                "private_key": "{{privateKey.Replace("\n", "\\n")}}",
                "client_email": "{{clientEmail}}",
                "token_uri": "https://oauth2.googleapis.com/token"
            }
            """;

            var credentials = Google.Apis.Auth.OAuth2.GoogleCredential
                .FromJson(credentialsJson)
                .CreateScoped("https://www.googleapis.com/auth/firebase.messaging");

            var token = await credentials.UnderlyingCredential
                .GetAccessTokenForRequestAsync(cancellationToken: cancellationToken);

            if (token != null)
            {
                lock (_lock)
                {
                    _cachedAccessToken = token;
                    _tokenExpiry = DateTime.UtcNow.AddMinutes(55);
                }

                _logger.LogDebug("Token de acceso FCM generado exitosamente");
                return token;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener token de acceso FCM");
        }

        return null;
    }

    private string MaskToken(string token)
    {
        if (string.IsNullOrEmpty(token) || token.Length < 8)
            return "***";

        return $"{token.Substring(0, 4)}...{token.Substring(token.Length - 4)}";
    }

    private string ExtractFcmErrorMessage(string errorContent)
    {
        try
        {
            if (string.IsNullOrEmpty(errorContent))
                return "Respuesta vacía de FCM";

            using var jsonDoc = JsonDocument.Parse(errorContent);

            if (jsonDoc.RootElement.TryGetProperty("error", out var errorElement))
            {
                if (errorElement.TryGetProperty("message", out var messageElement))
                    return messageElement.GetString() ?? "Error desconocido de FCM";

                if (errorElement.TryGetProperty("status", out var statusElement))
                    return statusElement.GetString() ?? "Error desconocido de FCM";
            }

            return "Error desconocido de FCM";
        }
        catch
        {
            return "No se pudo parsear el error de FCM";
        }
    }
}
