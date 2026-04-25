using CalcTarifa.Domain.Enums;
using CalcTarifa.Domain.DomainServices.Strategies;

namespace CalcTarifa.Domain.DomainServices
{
    public interface ITaxRateResolver
    {
        ICalculoTarifaStrategy Resolve(RegionEnvio region);
    }

    public class TaxRateResolver : ITaxRateResolver
    {
        private readonly IEnumerable<ICalculoTarifaStrategy> _strategies;

        public TaxRateResolver(IEnumerable<ICalculoTarifaStrategy> strategies)
        {
            _strategies = strategies;
        }

        public ICalculoTarifaStrategy Resolve(RegionEnvio region)
        {
            var strategy = _strategies.FirstOrDefault(s => s.AplicaPara(region));
            
            if (strategy == null)
                throw new CalcTarifa.Domain.DomainValidationException(
                    $"No existe una estrategia de cálculo definida para la región {region}.");

            return strategy;
        }
    }
}
