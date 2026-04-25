using CalcTarifa.Domain.Entities;
using CalcTarifa.Domain.Enums;
using CalcTarifa.Domain.ValueObjects;

namespace CalcTarifa.Domain.DomainServices.Strategies
{
    public class CalculoUS : CalculoRegionBase
    {
        public override RegionEnvio RegionSoportada => RegionEnvio.US;
    }
}
