using CalcTarifa.BusinessApplication.DTOs;

namespace CalcTarifa.BusinessApplication.Interfaces.UseCases
{
    public interface IConsultarTarifasUseCase
    {
        Task<IEnumerable<TarifaRegionResponse>> EjecutarAsync();
    }
}
