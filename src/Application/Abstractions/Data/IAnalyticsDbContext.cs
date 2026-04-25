using Domain.Analytics.AccesosDiaSemana;
using Domain.Analytics.AccesosIncidencias;
using Domain.Analytics.AccesosPorHora;
using Domain.Analytics.AccesosTipoUsuario;
using Microsoft.EntityFrameworkCore;

namespace Application.Abstractions.Data;

public interface IAnalyticsDbContext
{
    DbSet<AccesoTipoUsuario> AccesosTipoUsuario { get; }
    DbSet<AccesoPorHora> AccesosPorHora { get; }
    DbSet<AccesoDiaSemana> AccesosDiaSemana { get; }
    DbSet<AccesoIncidencia> AccesosIncidencias { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
