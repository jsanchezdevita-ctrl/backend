using SharedKernel;

namespace Application.Abstractions.Parametros;

public interface IParametroValidator
{
    Task<Result> ValidarJsonAsync(string llave, string json);
}