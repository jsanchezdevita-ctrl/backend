using SharedKernel;

namespace Domain.Parametros;

public static class ParametrosSchemaErrores
{
    public static Error NotFound(string llave) => Error.NotFound(
        "ParametrosSchema.NotFound",
        $"No se encontró schema para la llave = '{llave}'");

    public static Error NotFoundById(Guid id) => Error.NotFound(
        "ParametrosSchema.NotFoundById",
        $"No se encontró el schema con Id = '{id}'");

    public static Error LlaveAlreadyExists(string llave) => Error.Conflict(
        "ParametrosSchema.LlaveAlreadyExists",
        $"Ya existe un schema activo para la llave = '{llave}'");

    public static Error SchemaInvalido => Error.Validation(
        "ParametrosSchema.SchemaInvalido",
        "El schema proporcionado no es un JSON Schema válido");
}