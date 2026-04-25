namespace SharedKernel;
public static class TimeAgoHelper
{
    public static string GetTimeAgoText(DateTime fechaRegistro)
    {
        var diferencia = DateTime.UtcNow - fechaRegistro;

        if (diferencia.TotalMinutes < 1)
        {
            return "Hace menos de un minuto";
        }

        if (diferencia.TotalHours < 1)
        {
            var minutos = (int)diferencia.TotalMinutes;
            return $"Hace {minutos} minuto{(minutos != 1 ? "s" : "")}";
        }

        if (diferencia.TotalDays < 1)
        {
            var horas = (int)diferencia.TotalHours;
            return $"Hace {horas} hora{(horas != 1 ? "s" : "")}";
        }

        var dias = (int)diferencia.TotalDays;

        if (dias == 1)
        {
            return "Ayer";
        }

        if (dias < 7)
        {
            return $"Hace {dias} días";
        }

        if (dias < 30)
        {
            var semanas = dias / 7;
            return $"Hace {semanas} semana{(semanas != 1 ? "s" : "")}";
        }

        if (dias < 365)
        {
            var meses = dias / 30;
            return $"Hace {meses} mes{(meses != 1 ? "es" : "")}";
        }

        var años = dias / 365;
        return $"Hace {años} año{(años != 1 ? "s" : "")}";
    }
}