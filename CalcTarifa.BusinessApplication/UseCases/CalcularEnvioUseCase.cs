using CalcTarifa.BusinessApplication.DTOs;
using CalcTarifa.BusinessApplication.Interfaces.Persistence;
using CalcTarifa.BusinessApplication.Interfaces.UseCases;
using CalcTarifa.Domain.DomainServices;
using CalcTarifa.Domain.Entities;
using CalcTarifa.Domain.ValueObjects;
using Microsoft.Extensions.Logging;
using CalcTarifa.BusinessApplication.Mappings;
using CalcTarifa.Domain.Validations;

namespace CalcTarifa.BusinessApplication.UseCases
{
    // orquestador del flujo de calculo de envio
    public class CalcularEnvioUseCase : ICalcularEnvioUseCase
    {
        private readonly IUnitOfWork _uow;
        private readonly CalculationService _calculadora;
        private readonly Microsoft.Extensions.Logging.ILogger<CalcularEnvioUseCase> _logger;

        public CalcularEnvioUseCase(
            IUnitOfWork uow, 
            CalculationService calculadora,
            Microsoft.Extensions.Logging.ILogger<CalcularEnvioUseCase> logger)
        {
            _uow = uow;
            _calculadora = calculadora;
            _logger = logger;
        }

        public async Task<CalcularEnvioResponse> EjecutarAsync(CalcularEnvioRequest request)
        {
            try
            {
                _logger.LogInformation("Iniciando cálculo de envío para {Cliente}", request.EmailCliente);

                var peso = request.Unidad.ToLower() == "lb" 
                    ? PesoLb.Crear(request.Peso).ToKg() 
                    : PesoKg.Crear(request.Peso);

                var tarifa = await _uow.Tarifas.ObtenerPorRegionAsync(request.Region)
                    ?? throw new EntityNotFoundException("TarifaRegion", request.Region);

                var costo = _calculadora.Calcular(peso, tarifa);

                var registro = new RegistroCalculo(
                    request.NombreCliente,
                    request.EmailCliente,
                    peso,
                    request.Region,
                    tarifa.TarifaPorKg,
                    request.UserId);

                await _uow.RegistrosWrite.AgregarAsync(registro);
                await _uow.CommitAsync();

                _logger.LogInformation("Cálculo completado exitosamente: ID {RegistroId}", registro.Id);

                return registro.ToResponse();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al procesar el cálculo de envío para {Cliente}", request.EmailCliente);
                throw;
            }
        }
    }
}
