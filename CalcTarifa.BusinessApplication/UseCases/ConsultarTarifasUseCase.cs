using CalcTarifa.BusinessApplication.DTOs;
using CalcTarifa.BusinessApplication.Interfaces.Persistence;
using CalcTarifa.BusinessApplication.Interfaces.UseCases;
using CalcTarifa.BusinessApplication.Mappings;

namespace CalcTarifa.BusinessApplication.UseCases
{
    public class ConsultarTarifasUseCase : IConsultarTarifasUseCase
    {
        private readonly IUnitOfWork _uow;

        public ConsultarTarifasUseCase(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IEnumerable<TarifaRegionResponse>> EjecutarAsync()
        {
            var tarifas = await _uow.Tarifas.ObtenerTodasActivasAsync();
            return tarifas.Select(t => t.ToResponse());
        }
    }
}
