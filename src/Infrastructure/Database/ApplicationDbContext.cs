using Application.Abstractions.Data;
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
using Infrastructure.DomainEvents;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using System.Reflection;

namespace Infrastructure.Database;

public sealed class ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options,
    IDomainEventsDispatcher domainEventsDispatcher)
    : DbContext(options), IApplicationDbContext
{
    public DbSet<Usuario> Usuarios { get; set; }

    public DbSet<Rol> Roles { get; set; }

    public DbSet<UsuarioRol> UsuariosRoles { get; set; }

    public DbSet<PuntoControl> PuntosControl { get; set; }

    public DbSet<RegistroIngresoEgreso> RegistrosIngresosEgresos { get; set; }

    public DbSet<Parametro> Parametros { get; set; }

    public DbSet<Dispositivo> Dispositivos { get; set; }

    public DbSet<RefreshToken> RefreshTokens { get; set; }

    public DbSet<Permiso> Permisos { get; set; }

    public DbSet<RolPermiso> RolesPermisos { get; set; }

    public DbSet<DispositivoConfiguracion> DispositivoConfiguraciones { get; set; }

    public DbSet<ParametroHistorial> ParametrosHistorial { get; set; }

    public DbSet<ParametroSchema> ParametroSchemas { get; set; }

    public DbSet<EstadoRegistro> EstadosRegistro { get; set; }

    public DbSet<RolUI> RolesUI { get; set; }

    public DbSet<Zona> Zonas { get; set; }

    public DbSet<ZonaRol> ZonasRoles { get; set; }
    
    public DbSet<ZonaPuntoControl> ZonasPuntosControl { get; set; }
    
    public DbSet<QrToken> QrTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(ApplicationDbContext).Assembly,
             type => type.Namespace?.Contains("Analytics") == false);

        modelBuilder.HasDefaultSchema(Schemas.Default);

        ApplySoftDeleteGlobalFilter(modelBuilder);

        modelBuilder.Model.GetEntityTypes()
            .Where(t => typeof(Entity).IsAssignableFrom(t.ClrType))
            .ToList()
            .ForEach(t =>
            {
                modelBuilder.Entity(t.ClrType)
                    .Property(nameof(Entity.CreatedAt))
                    .HasColumnName("created_at")
                    .HasColumnType("timestamptz")
                    .HasDefaultValueSql("now() at time zone 'utc'")
                    .ValueGeneratedOnAdd();
            });
    }

    private static void ApplySoftDeleteGlobalFilter(ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(Entity).IsAssignableFrom(entityType.ClrType))
            {
                var method = typeof(ApplicationDbContext)
                    .GetMethod(nameof(ApplySoftDeleteFilter),
                             BindingFlags.Static | BindingFlags.NonPublic)?
                    .MakeGenericMethod(entityType.ClrType);

                method?.Invoke(null, new object[] { modelBuilder });
            }
        }
    }

    private static void ApplySoftDeleteFilter<T>(ModelBuilder modelBuilder)
    where T : Entity
    {
        modelBuilder.Entity<T>().HasQueryFilter(e => !e.Deleted);
    }



    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // When should you publish domain events?
        //
        // 1. BEFORE calling SaveChangesAsync
        //     - domain events are part of the same transaction
        //     - immediate consistency
        // 2. AFTER calling SaveChangesAsync
        //     - domain events are a separate transaction
        //     - eventual consistency
        //     - handlers can fail

        int result = await base.SaveChangesAsync(cancellationToken);

        await PublishDomainEventsAsync();

        return result;
    }

    private async Task PublishDomainEventsAsync()
    {
        var domainEvents = ChangeTracker
            .Entries<Entity>()
            .Select(entry => entry.Entity)
            .SelectMany(entity =>
            {
                List<IDomainEvent> domainEvents = entity.DomainEvents;

                entity.ClearDomainEvents();

                return domainEvents;
            })
            .ToList();

        await domainEventsDispatcher.DispatchAsync(domainEvents);
    }
}
