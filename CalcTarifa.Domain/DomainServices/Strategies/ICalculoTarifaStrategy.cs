using CalcTarifa.Domain.Entities;
using CalcTarifa.Domain.Enums;
using CalcTarifa.Domain.ValueObjects;

namespace CalcTarifa.Domain.DomainServices.Strategies
{
    public interface ICalculoTarifaStrategy
    {
        bool AplicaPara(RegionEnvio region);
        decimal Calcular(PesoKg peso, TarifaRegion tarifa);
    }
}
