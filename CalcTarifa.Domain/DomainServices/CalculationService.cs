using CalcTarifa.Domain.Entities;
using CalcTarifa.Domain.ValueObjects;

namespace CalcTarifa.Domain.DomainServices
{
    // coordinador principal de calculo
    public class CalculationService
    {
        private readonly ITaxRateResolver _resolver;

        public CalculationService(ITaxRateResolver resolver)
        {
            _resolver = resolver;
        }

        public decimal Calcular(PesoKg peso, TarifaRegion tarifa)
        {
            if (!tarifa.Activo)
                throw new CalcTarifa.Domain.DomainValidationException(
                    $"La región '{tarifa.NombrePais}' no está disponible para envíos en este momento.");

            // Resolvemos el componente de cálculo adecuado (Tax Rate Resolver)
            var strategy = _resolver.Resolve(tarifa.Region);

            // Ejecutamos el cálculo (Tax Calculator)
            return strategy.Calcular(peso, tarifa);
        }
    }
}
