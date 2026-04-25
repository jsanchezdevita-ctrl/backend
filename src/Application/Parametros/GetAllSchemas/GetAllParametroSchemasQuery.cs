using Application.Abstractions.Messaging;
using SharedKernel;

namespace Application.Parametros.GetAllSchemas;

public sealed record GetAllParametroSchemasQuery(
    int Page,
    int PageSize,
    string? SearchTerm) : IQuery<PagedResponse<ParametroSchemaResponse>>;

public record ParametroSchemaResponse(
    Guid Id,
    string Llave,
    string Schema,
    DateTime FechaCreacion,
    DateTime FechaActualizacion);