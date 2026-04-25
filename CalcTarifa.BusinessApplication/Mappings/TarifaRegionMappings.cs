using CalcTarifa.BusinessApplication.DTOs;
using CalcTarifa.Domain.Entities;

namespace CalcTarifa.BusinessApplication.Mappings
{
    public static class TarifaRegionMappings
    {
        public static TarifaRegionResponse ToResponse(this TarifaRegion t) => new()
        {
            Codigo      = t.Region.ToString(),
            NombrePais  = t.NombrePais,
            TarifaPorKg = t.TarifaPorKg
        };
    }
}
