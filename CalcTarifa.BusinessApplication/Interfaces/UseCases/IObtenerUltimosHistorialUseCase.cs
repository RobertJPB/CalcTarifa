using CalcTarifa.BusinessApplication.DTOs;

namespace CalcTarifa.BusinessApplication.Interfaces.UseCases
{
    public interface IObtenerUltimosHistorialUseCase
    {
        Task<IEnumerable<HistorialCalculoResponse>> EjecutarAsync(int cantidad = 10, string? userId = null);
    }
}
