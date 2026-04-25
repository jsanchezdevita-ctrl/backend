using SharedKernel;

namespace Application.Abstractions.Notifications;

public interface IFcmNotificationService
{
    Task<Result> SendNotificationAsync(
        string fcmToken,
        string title,
        string body,
        string type,
        CancellationToken cancellationToken = default);
}