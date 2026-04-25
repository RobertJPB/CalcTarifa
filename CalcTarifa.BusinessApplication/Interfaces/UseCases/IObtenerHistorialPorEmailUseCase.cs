using CalcTarifa.BusinessApplication.DTOs;

namespace CalcTarifa.BusinessApplication.Interfaces.UseCases
{
    public interface IObtenerHistorialPorEmailUseCase
    {
        Task<IEnumerable<HistorialCalculoResponse>> EjecutarAsync(string email);
    }
}
