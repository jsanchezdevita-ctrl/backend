using Application.Abstractions.Authentication;
using Domain.Enums;
using Domain.EstadosRegistro;
using Domain.Parametros;
using Domain.Permisos;
using Domain.Roles;
using Domain.RolesPermisos;
using Domain.RolesUI;
using Domain.Usuarios;
using Domain.UsuariosRoles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Database;

public class DbInitializer(
    IServiceProvider serviceProvider,
    ILogger<DbInitializer> logger,
    IPasswordHasher passwordHasher) : IHostedService
{
    public const string AdminUserEmail = "admin@admin.ban.com";
    public const string AdminUserPassword = "admin";

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        //await dbContext.Database.EnsureDeletedAsync(cancellationToken);
        //await dbContext.Database.EnsureCreatedAsync(cancellationToken);

        logger.LogInformation("Running initial migration");
        //await dbContext.Database.MigrateAsync(cancellationToken);

        await SeedRolesAsync(dbContext);
        await SeedPermissionsAsync(dbContext);
        await SeedUserAdminAsync(dbContext, passwordHasher);
        await SeedRolePermissionsAsync(dbContext);
        await SeedRoleUIsAsync(dbContext);
        await SeedParametrosAndSchemasAsync(dbContext);
        await SeedEstadosRegistroAsync(dbContext);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    private static async Task SeedRolesAsync(ApplicationDbContext dbContext)
    {
        if (!await dbContext.Roles.AnyAsync())
        {
            var roles = new List<Rol>
            {
                new() { Id = Guid.NewGuid(), NombreRol = "Administrador", Descripcion = "Administrador del sistema" },
                new() { Id = Guid.NewGuid(), NombreRol = "Seguridad", Descripcion = "Personal de seguridad" },
                new() { Id = Guid.NewGuid(), NombreRol = "Direccion", Descripcion = "Dirección administrativa" },
                new() { Id = Guid.NewGuid(), NombreRol = "Alumno", Descripcion = "Estudiante Inscripto" },
                new() { Id = Guid.NewGuid(), NombreRol = "Docente", Descripcion = "Personal Docente" },
                new() { Id = Guid.NewGuid(), NombreRol = "Funcionario", Descripcion = "Personal Administrativo" }
            };

            await dbContext.Roles.AddRangeAsync(roles);
            await dbContext.SaveChangesAsync();
        }
    }

    private static async Task SeedPermissionsAsync(ApplicationDbContext dbContext)
    {
        if (!await dbContext.Permisos.AnyAsync())
        {
            var permissions = new List<Permiso>
        {
            new() { Id = Guid.NewGuid(), Nombre = "admin.all" },
            new() { Id = Guid.NewGuid(), Nombre = "usuarios.read" },
            new() { Id = Guid.NewGuid(), Nombre = "usuarios.write" }
        };

            await dbContext.Permisos.AddRangeAsync(permissions);
            await dbContext.SaveChangesAsync();
        }
    }
    private static async Task SeedUserAdminAsync(ApplicationDbContext dbContext, IPasswordHasher passwordHasher)
    {
        if (!await dbContext.Usuarios.AnyAsync(u => u.Email == AdminUserEmail))
        {
            var passwordHash = passwordHasher.Hash(AdminUserPassword);

            var user = new Usuario
            {
                Id = Guid.NewGuid(),
                Email = AdminUserEmail,
                Nombre = "Administrador",
                Apellido = "Root",
                PasswordHash = passwordHash,
                Estado = UsuarioState.Habilitado,
                FechaRegistro = DateTime.UtcNow,
                FechaUltimaModificacion = DateTime.UtcNow
            };

            await dbContext.Usuarios.AddAsync(user);

            var adminRole = await dbContext.Roles.FirstOrDefaultAsync(r => r.NombreRol == "Administrador");
            if (adminRole != null)
            {
                var usuarioRol = new UsuarioRol
                {
                    Id = Guid.NewGuid(),
                    UsuarioId = user.Id,
                    RolId = adminRole.Id,
                    FechaAsignacion = DateTime.UtcNow
                };
                await dbContext.UsuariosRoles.AddAsync(usuarioRol);
            }

            await dbContext.SaveChangesAsync();
        }
    }


    private static async Task SeedRolePermissionsAsync(ApplicationDbContext dbContext)
    {
        if (!await dbContext.RolesPermisos.AnyAsync())
        {
            var adminRole = await dbContext.Roles.FirstOrDefaultAsync(r => r.NombreRol == "Administrador");
            var adminAllPermission = await dbContext.Permisos.FirstOrDefaultAsync(p => p.Nombre == "admin.all");
            var userReadPermission = await dbContext.Permisos.FirstOrDefaultAsync(p => p.Nombre == "usuarios.read");
            var userWritePermission = await dbContext.Permisos.FirstOrDefaultAsync(p => p.Nombre == "usuarios.write");

            if (adminRole != null && userReadPermission != null && userWritePermission != null)
            {
                var rolePermissions = new List<RolPermiso>
            {
                new() { Id = Guid.NewGuid(), RolId = adminRole.Id, PermisoId = adminAllPermission.Id },
                new() { Id = Guid.NewGuid(), RolId = adminRole.Id, PermisoId = userReadPermission.Id },
                new() { Id = Guid.NewGuid(), RolId = adminRole.Id, PermisoId = userWritePermission.Id }
            };

                await dbContext.RolesPermisos.AddRangeAsync(rolePermissions);
                await dbContext.SaveChangesAsync();
            }
        }
    }

    private static async Task SeedParametrosAndSchemasAsync(ApplicationDbContext dbContext)
    {
        if (!await dbContext.Parametros.AnyAsync() && !await dbContext.ParametroSchemas.AnyAsync())
        {
            var parametrosData = new[]
            {
            new
            {
                Llave = "seguridad_qr",
                Valor = """{"vigenciaQRHoras":24,"intervaloRenovacionMinutos":5,"nivelCifrado":"AES-256"}""",
                Descripcion = "Configuración de seguridad para códigos QR",
                Schema = """{"type":"object","properties":{"vigenciaQRHoras":{"type":"integer","minimum":1,"maximum":720},"intervaloRenovacionMinutos":{"type":"integer","minimum":1,"maximum":60},"nivelCifrado":{"type":"string","enum":["AES-128","AES-192","AES-256"]}},"required":["vigenciaQRHoras","intervaloRenovacionMinutos","nivelCifrado"],"additionalProperties":false}"""
            },
            new
            {
                Llave = "politicas_acceso",
                Valor = """{"registroAccesos":true,"notificarAccesosNoAutorizados":true,"bloqueoAutomaticoPuertas":true,"tiempoBloqueoSegundos":5}""",
                Descripcion = "Políticas de acceso al sistema",
                Schema = """{"type":"object","properties":{"registroAccesos":{"type":"boolean"},"notificarAccesosNoAutorizados":{"type":"boolean"},"bloqueoAutomaticoPuertas":{"type":"boolean"},"tiempoBloqueoSegundos":{"type":"integer","minimum":1,"maximum":300}},"required":["registroAccesos","notificarAccesosNoAutorizados","bloqueoAutomaticoPuertas","tiempoBloqueoSegundos"],"additionalProperties":false}"""
            },
            new
            {
                Llave = "autenticacion",
                Valor = """{"autenticacionDosFactores":true,"tiempoSesionMinutos":30,"intentosMaximosLogin":3,"bloquearCuentaDespuesIntentos":true}""",
                Descripcion = "Configuración de autenticación",
                Schema = """{"type":"object","properties":{"autenticacionDosFactores":{"type":"boolean"},"tiempoSesionMinutos":{"type":"integer","minimum":1,"maximum":1440},"intentosMaximosLogin":{"type":"integer","minimum":1,"maximum":10},"bloquearCuentaDespuesIntentos":{"type":"boolean"}},"required":["autenticacionDosFactores","tiempoSesionMinutos","intentosMaximosLogin","bloquearCuentaDespuesIntentos"],"additionalProperties":false}"""
            },
            new
            {
                Llave = "configuracion_adicional",
                Valor = """{"zonaHoraria":"America/Asuncion","retencionLogsDias":90,"frecuenciaBackup":"Diario"}""",
                Descripcion = "Configuración adicional del sistema",
                Schema = """{"type":"object","properties":{"zonaHoraria":{"type":"string"},"retencionLogsDias":{"type":"integer","minimum":1,"maximum":3650},"frecuenciaBackup":{"type":"string","enum":["Cada hora","Diario","Semanal","Mensual"]}},"required":["zonaHoraria","retencionLogsDias","frecuenciaBackup"],"additionalProperties":false}"""
            }
        };

            var parametros = new List<Parametro>();
            var schemas = new List<ParametroSchema>();

            foreach (var data in parametrosData)
            {
                var parametroId = Guid.NewGuid();
                var schemaId = Guid.NewGuid();

                parametros.Add(new Parametro
                {
                    Id = parametroId,
                    Llave = data.Llave,
                    Valor = data.Valor,
                    Descripcion = data.Descripcion,
                    FechaActualizacion = DateTime.UtcNow,
                    ActualizadoPor = "System",
                    Version = 1
                });

                schemas.Add(new ParametroSchema
                {
                    Id = schemaId,
                    Llave = data.Llave,
                    Schema = data.Schema,
                    FechaCreacion = DateTime.UtcNow,
                    FechaActualizacion = DateTime.UtcNow
                });
            }

            await dbContext.Parametros.AddRangeAsync(parametros);
            await dbContext.ParametroSchemas.AddRangeAsync(schemas);
            await dbContext.SaveChangesAsync();
        }
    }

    private static async Task SeedEstadosRegistroAsync(ApplicationDbContext dbContext)
    {
        if (!await dbContext.EstadosRegistro.AnyAsync())
        {
            var estados = new List<EstadoRegistro>
        {
            new() { Id = Guid.NewGuid(), Descripcion = "Autorizado" },
            new() { Id = Guid.NewGuid(), Descripcion = "Denegado" }
        };

            await dbContext.EstadosRegistro.AddRangeAsync(estados);
            await dbContext.SaveChangesAsync();
        }
    }

    private static async Task SeedRoleUIsAsync(ApplicationDbContext dbContext)
    {
        if (!await dbContext.RolesUI.AnyAsync())
        {
            var roles = await dbContext.Roles.ToListAsync();

            var roleUIs = new List<RolUI>();

            var roleColorMapping = new Dictionary<string, (string textColor, string backgroundColor)>
        {
            { "Administrador", ("#1E40AF", "#E0F2FE") },
            { "Seguridad", ("#9A3412", "#FFEDD5") },    
            { "Direccion", ("#14532D", "#DCFCE7") },     
            { "Alumno", ("#193CB8", "#DBEAFE") },        
            { "Docente", ("#016630", "#DBFCE7") },      
            { "Funcionario", ("#AE36B0", "#F3E8FF") }    
        };

            foreach (var role in roles)
            {
                if (roleColorMapping.TryGetValue(role.NombreRol, out var colors))
                {
                    roleUIs.Add(new RolUI
                    {
                        Id = Guid.NewGuid(),
                        RolId = role.Id,
                        TextColor = colors.textColor,
                        BackgroundColor = colors.backgroundColor
                    });
                }
                else
                {
                    roleUIs.Add(new RolUI
                    {
                        Id = Guid.NewGuid(),
                        RolId = role.Id,
                        TextColor = "#000000",
                        BackgroundColor = "#FFFFFF"
                    });
                }
            }

            await dbContext.RolesUI.AddRangeAsync(roleUIs);
            await dbContext.SaveChangesAsync();
        }
    }

}
