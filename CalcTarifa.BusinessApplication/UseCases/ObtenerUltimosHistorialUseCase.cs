using CalcTarifa.BusinessApplication.DTOs;
using CalcTarifa.BusinessApplication.Interfaces.Persistence;
using CalcTarifa.BusinessApplication.Interfaces.UseCases;
using CalcTarifa.BusinessApplication.Mappings;

namespace CalcTarifa.BusinessApplication.UseCases
{
    public class ObtenerUltimosHistorialUseCase : IObtenerUltimosHistorialUseCase
    {
        private readonly IUnitOfWork _uow;

        public ObtenerUltimosHistorialUseCase(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IEnumerable<HistorialCalculoResponse>> EjecutarAsync(int cantidad = 10, string? userId = null)
        {
            var registros = string.IsNullOrEmpty(userId)
                ? await _uow.RegistrosRead.ObtenerUltimosAsync(cantidad)
                : await _uow.RegistrosRead.ObtenerPorUsuarioAsync(userId, cantidad);

            return registros.Select(r => r.ToHistorialResponse());
        }
    }
}
