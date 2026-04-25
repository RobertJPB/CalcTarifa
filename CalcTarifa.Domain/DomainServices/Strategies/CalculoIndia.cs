using CalcTarifa.Domain.Entities;
using CalcTarifa.Domain.Enums;
using CalcTarifa.Domain.ValueObjects;

namespace CalcTarifa.Domain.DomainServices.Strategies
{
    public class CalculoIndia : CalculoRegionBase
    {
        public override RegionEnvio RegionSoportada => RegionEnvio.India;
    }
}
