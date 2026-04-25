using SharedKernel;

namespace Domain.Parametros;

public static class ParametrosErrores
{
    public static Error NotFound(string llave) => Error.NotFound(
        "Parametros.NotFound",
        $"No se encontró el parámetro con llave = '{llave}'");

    public static Error NotFoundById(Guid id) => Error.NotFound(
        "Parametros.NotFoundById",
        $"No se encontró el parámetro con Id = '{id}'");

    public static Error LlaveAlreadyExists(string llave) => Error.Conflict(
        "Parametros.LlaveAlreadyExists",
        $"Ya existe un parámetro con la llave = '{llave}'");

    public static Error InvalidJsonFormat(string llave) => Error.Validation(
        "Parametros.InvalidJsonFormat",
        $"El valor del parámetro '{llave}' no tiene un formato JSON válido");

    public static Error LlaveRequired => Error.Validation(
        "Parametros.LlaveRequired",
        "La llave del parámetro es requerida");

    public static Error ValorRequired => Error.Validation(
        "Parametros.ValorRequired",
        "El valor del parámetro es requerido");

    public static Error JsonNotSupportedSchema(string llave, string errores) => Error.Validation(
    "Parametros.JsonNotSupportedSchema",
    $"El JSON para '{llave}' no cumple el schema: {errores}");

    public static Error NotFoundByKey(String key) => Error.NotFound(
    "Parametros.NotFoundByKey",
    $"No se encontró el parámetro con Key = '{key}'");
}