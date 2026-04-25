using CalcTarifa.BusinessApplication.DTOs;
using CalcTarifa.Domain.Entities;

namespace CalcTarifa.BusinessApplication.Mappings
{
    public static class RegistroCalculoMappings
    {
        public static CalcularEnvioResponse ToResponse(this RegistroCalculo r) => new()
        {
            Id            = r.Id,
            NombreCliente = r.NombreCliente,
            EmailCliente  = r.EmailCliente,
            PesoKg        = r.PesoKg,
            Region        = r.Region.ToString(),
            TarifaPorKg   = r.TarifaAplicada,
            CostoTotal    = r.CostoTotal,
            UserId        = r.UserId,
            FechaCalculo  = r.FechaCalculo
        };

        public static HistorialCalculoResponse ToHistorialResponse(this RegistroCalculo r) => new()
        {
            Id            = r.Id,
            NombreCliente = r.NombreCliente,
            PesoKg        = r.PesoKg,
            Region        = r.Region.ToString(),
            CostoTotal    = r.CostoTotal,
            UserId        = r.UserId,
            FechaCalculo  = r.FechaCalculo
        };
    }
}
