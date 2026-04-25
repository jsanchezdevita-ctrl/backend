namespace SharedKernel;

public static class DateTimeExtensions
{
    public static (string Completo, string Corto) ToDiaSemanaEspanol(this DateTime fecha)
    {
        return fecha.DayOfWeek switch
        {
            DayOfWeek.Monday => ("Lunes", "Lun"),
            DayOfWeek.Tuesday => ("Martes", "Mar"),
            DayOfWeek.Wednesday => ("Miércoles", "Mié"),
            DayOfWeek.Thursday => ("Jueves", "Jue"),
            DayOfWeek.Friday => ("Viernes", "Vie"),
            DayOfWeek.Saturday => ("Sábado", "Sáb"),
            DayOfWeek.Sunday => ("Domingo", "Dom"),
            _ => throw new ArgumentException($"Día de la semana no válido: {fecha.DayOfWeek}")
        };
    }

    public static (DateTime StartOfWeek, DateTime EndOfWeek) GetCurrentWeekRange()
    {
        var today = DateTime.UtcNow.Date;
        int diff = (7 + (today.DayOfWeek - DayOfWeek.Monday)) % 7;
        var startOfWeek = today.AddDays(-1 * diff).Date;
        var endOfWeek = startOfWeek.AddDays(6).Date.AddDays(1).AddTicks(-1);

        return (startOfWeek, endOfWeek);
    }

    public static (DateTime StartOfWeek, DateTime EndOfWeek) GetPreviousWeekRange()
    {
        var (startOfCurrentWeek, endOfCurrentWeek) = GetCurrentWeekRange();
        var startOfPreviousWeek = startOfCurrentWeek.AddDays(-7);
        var endOfPreviousWeek = startOfPreviousWeek.AddDays(6).Date.AddDays(1).AddTicks(-1);

        return (startOfPreviousWeek, endOfPreviousWeek);
    }
}