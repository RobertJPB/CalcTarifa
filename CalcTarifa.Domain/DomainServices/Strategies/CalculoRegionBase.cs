using CalcTarifa.Domain.Entities;
using CalcTarifa.Domain.Enums;
using CalcTarifa.Domain.ValueObjects;

namespace CalcTarifa.Domain.DomainServices.Strategies
{
    // base para estrategias de calculo por region
    public abstract class CalculoRegionBase : ICalculoTarifaStrategy
    {
        // region que soporta el componente
        public abstract RegionEnvio RegionSoportada { get; }

        public bool AplicaPara(RegionEnvio region) => region == RegionSoportada;

        // logica de calculo base (puede ser sobreescrita)
        public virtual decimal Calcular(PesoKg peso, TarifaRegion tarifa)
        {
            // Regla de negocio general: peso × tarifa por kg
            return Math.Round(peso.Valor * tarifa.TarifaPorKg, 2);
        }
    }
}
