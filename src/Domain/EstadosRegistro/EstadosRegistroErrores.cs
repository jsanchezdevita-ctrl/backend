using SharedKernel;

namespace Domain.EstadosRegistro;

public static class EstadosRegistroErrores
{
    public static Error NotFound(Guid estadoRegistroId) => Error.NotFound(
        "EstadosRegistro.NotFound",
        $"No se encontró el estado de registro con Id = '{estadoRegistroId}'");

    public static Error DescripcionNotUnique => Error.Conflict(
        "EstadosRegistro.DescripcionNotUnique",
        "La descripción del estado de registro ya existe");
}