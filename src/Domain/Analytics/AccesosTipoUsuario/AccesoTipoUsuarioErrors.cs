using SharedKernel;

namespace Domain.Analytics.AccesosTipoUsuario;

public static class AccesoTipoUsuarioErrors
{
    public static Error NotFound(Guid accesoId) => Error.NotFound(
        "AccesosTipoUsuario.NotFound",
        $"No se encontró el registro de acceso con Id = '{accesoId}'");

    public static readonly Error DuplicateEntry = Error.Conflict(
        "AccesosTipoUsuario.DuplicateEntry",
        "Ya existe un registro para esta fecha y tipo de usuario");
}