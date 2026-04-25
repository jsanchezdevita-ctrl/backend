using Domain.Authentication.RefreshTokens;
using Domain.Dispositivos;
using Domain.EstadosRegistro;
using Domain.Parametros;
using Domain.Permisos;
using Domain.PuntosControl;
using Domain.QrTokens;
using Domain.RegistrosIngresosEgresos;
using Domain.Roles;
using Domain.RolesPermisos;
using Domain.RolesUI;
using Domain.Usuarios;
using Domain.UsuariosRoles;
using Domain.Zonas;
using Domain.ZonasPuntosControl;
using Domain.ZonasRoles;
using Microsoft.EntityFrameworkCore;

namespace Application.Abstractions.Data;

public interface IApplicationDbContext
{
    DbSet<Usuario> Usuarios { get; }
    DbSet<Rol> Roles { get; }
    DbSet<UsuarioRol> UsuariosRoles { get; }
    DbSet<PuntoControl> PuntosControl { get; }
    DbSet<RegistroIngresoEgreso> RegistrosIngresosEgresos { get; }
    DbSet<Parametro> Parametros { get; }
    DbSet<Dispositivo> Dispositivos { get; }
    DbSet<RefreshToken> RefreshTokens { get; }
    DbSet<Permiso> Permisos { get; set; }
    DbSet<RolPermiso> RolesPermisos { get; set; }
    DbSet<DispositivoConfiguracion> DispositivoConfiguraciones { get; set; }
    DbSet<ParametroHistorial> ParametrosHistorial { get; set; }
    DbSet<ParametroSchema> ParametroSchemas { get; set; }
    DbSet<EstadoRegistro> EstadosRegistro { get; set; }
    DbSet<RolUI> RolesUI { get; set; }
    DbSet<Zona> Zonas { get; set; }
    DbSet<ZonaRol> ZonasRoles { get; set; }
    DbSet<ZonaPuntoControl> ZonasPuntosControl { get; set; }
    DbSet<QrToken> QrTokens { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
