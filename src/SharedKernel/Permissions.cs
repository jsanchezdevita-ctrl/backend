namespace SharedKernel;

public static class Permissions
{
    public const string Admin = "admin.all";
    public const string Usuarios = "usuarios.all";

    public const string UsuariosAccess = "usuarios.access";
    public const string UsuariosRead = "usuarios.read";
    public const string UsuariosCreate = "usuarios.create";
    public const string UsuariosUpdate = "usuarios.update";
    public const string UsuariosDelete = "usuarios.delete";

    public const string DispositivosAccess = "dispositivos.access";
    public const string DispositivosRead = "dispositivos.read";
    public const string DispositivosCreate = "dispositivos.create";

    public const string RolesManage = "roles.manage";
    public const string PermisosManage = "permisos.manage";

    public const string ZonasEstacionamientoAccess = "zonas.estacionamiento.access";
    public const string PuntosControlAccess = "puntos.control.access";
}