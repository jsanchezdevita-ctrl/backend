using Application.Abstractions.Data;
using Domain.Analytics.AccesosDiaSemana;
using Domain.Analytics.AccesosIncidencias;
using Domain.Analytics.AccesosPorHora;
using Domain.Analytics.AccesosTipoUsuario;
using Infrastructure.Database;
using Infrastructure.DomainEvents;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Infrastructure.Analytics;

public sealed class AnalyticDbContext(
    DbContextOptions<AnalyticDbContext> options,
    IDomainEventsDispatcher domainEventsDispatcher)
    : DbContext(options), IAnalyticsDbContext
{
    public DbSet<AccesoTipoUsuario> AccesosTipoUsuario { get; set; }
    public DbSet<AccesoPorHora> AccesosPorHora { get; set; }
    public DbSet<AccesoDiaSemana> AccesosDiaSemana { get; set; }
    public DbSet<AccesoIncidencia> AccesosIncidencias { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(AnalyticDbContext).Assembly,
            type => type.Namespace?.Contains("Analytics") == true);

        modelBuilder.HasDefaultSchema(Schemas.Default);

    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        int result = await base.SaveChangesAsync(cancellationToken);

        await PublishDomainEventsAsync();

        return result;
    }

    private async Task PublishDomainEventsAsync()
    {
        var domainEvents = ChangeTracker
            .Entries<AnalyticsEntity>()
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
