using CalcTarifa.BusinessApplication.DTOs;

namespace CalcTarifa.BusinessApplication.Interfaces.UseCases
{
    public interface ICalcularEnvioUseCase
    {
        Task<CalcularEnvioResponse> EjecutarAsync(CalcularEnvioRequest request);
    }
}
