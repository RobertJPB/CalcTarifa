using CalcTarifa.Domain.Enums;
using CalcTarifa.Domain.Validations;

namespace CalcTarifa.Domain.Entities
{
    // tarifa por kg vigente para una region
    public class TarifaRegion
    {
        public int         Id          { get; private set; }
        public RegionEnvio Region      { get; private set; }
        public string      NombrePais  { get; private set; } = null!;
        public decimal     TarifaPorKg { get; private set; }
        public bool        Activo      { get; private set; }

        protected TarifaRegion() { }

        public TarifaRegion(RegionEnvio region, string nombrePais, decimal tarifaPorKg)
        {
            if (string.IsNullOrWhiteSpace(nombrePais))
                throw new DomainValidationException("El nombre del país es obligatorio.");
            if (tarifaPorKg <= 0)
                throw new DomainValidationException("La tarifa debe ser mayor a cero.");

            Region      = region;
            NombrePais  = nombrePais.Trim();
            TarifaPorKg = tarifaPorKg;
            Activo      = true;
        }

        public void ActualizarTarifa(decimal nuevaTarifa)
        {
            if (nuevaTarifa <= 0)
                throw new DomainValidationException("La tarifa debe ser mayor a cero.");
            TarifaPorKg = nuevaTarifa;
        }

        public void Desactivar() => Activo = false;
        public void Activar()   => Activo = true;
    }
}
