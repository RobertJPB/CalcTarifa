using CalcTarifa.BusinessApplication.DTOs;
using CalcTarifa.BusinessApplication.Interfaces.Persistence;
using CalcTarifa.BusinessApplication.Interfaces.UseCases;
using CalcTarifa.BusinessApplication.Mappings;
using CalcTarifa.Domain.Validations;

namespace CalcTarifa.BusinessApplication.UseCases
{
    public class ObtenerHistorialPorEmailUseCase : IObtenerHistorialPorEmailUseCase
    {
        private readonly IUnitOfWork _uow;

        public ObtenerHistorialPorEmailUseCase(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IEnumerable<HistorialCalculoResponse>> EjecutarAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new DomainValidationException("El email es obligatorio.");

            var registros = await _uow.RegistrosRead.ObtenerPorEmailAsync(email.Trim().ToLowerInvariant());
            return registros.Select(r => r.ToHistorialResponse());
        }
    }
}
