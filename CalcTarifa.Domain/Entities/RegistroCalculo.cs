using CalcTarifa.Domain.Enums;
using CalcTarifa.Domain.ValueObjects;
using CalcTarifa.Domain.Validations;

namespace CalcTarifa.Domain.Entities
{
    // registra cada calculo de tarifa realizado
    public class RegistroCalculo
    {
        public int       Id             { get; private set; }
        public string?   UserId         { get; private set; }   // null = cálculo anónimo
        public string    NombreCliente  { get; private set; } = string.Empty;
        public string    EmailCliente   { get; private set; } = string.Empty;
        public decimal   PesoKg         { get; private set; }
        public RegionEnvio Region       { get; private set; }
        public decimal   TarifaAplicada { get; private set; }
        public decimal   CostoTotal     { get; private set; }
        public DateTime  FechaCalculo   { get; private set; }

        protected RegistroCalculo() { }

        public RegistroCalculo(
            string      nombreCliente,
            string      emailCliente,
            PesoKg      peso,
            RegionEnvio region,
            decimal     tarifaAplicada,
            string?     userId = null)
        {
            if (!Enum.IsDefined(typeof(RegionEnvio), region))
                throw new DomainValidationException("La región especificada no es válida.");

            NombreCliente  = nombreCliente?.Trim() ?? string.Empty;
            EmailCliente   = emailCliente?.Trim().ToLowerInvariant() ?? string.Empty;
            PesoKg         = peso.Valor;
            Region         = region;
            TarifaAplicada = tarifaAplicada;
            CostoTotal     = Math.Round(peso.Valor * tarifaAplicada, 2);
            FechaCalculo   = DateTime.UtcNow;
            UserId         = userId;
        }
    }
}
