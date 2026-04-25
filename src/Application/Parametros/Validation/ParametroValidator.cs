using Application.Abstractions.Data;
using Application.Abstractions.Parametros;
using Domain.Parametros;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Parametros.Validation;

public class ParametroValidator(IApplicationDbContext context) : IParametroValidator
{
    public async Task<Result> ValidarJsonAsync(string llave, string json)
    {
        var schema = await context.ParametroSchemas
            .FirstOrDefaultAsync(ps => ps.Llave == llave);

        if (schema == null)
            return Result.Failure(ParametrosSchemaErrores.NotFound(llave));

        var (isValid, errors) = JsonSchemaValidator.Validate(json, schema.Schema);

        return isValid ?
            Result.Success() :
            Result.Failure(ParametrosErrores.JsonNotSupportedSchema(llave, string.Join("; ", errors)));
    }
}