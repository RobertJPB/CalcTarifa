using System.ComponentModel.DataAnnotations;
using CalcTarifa.Domain.Enums;

namespace CalcTarifa.BusinessApplication.DTOs
{
    // ── Request ────────────────────────────────────────────────────────────────

    public class CalcularEnvioRequest
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(200)]
        [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "El nombre solo debe contener letras y espacios.")]
        public string NombreCliente { get; set; } = string.Empty;

        [Required(ErrorMessage = "El email es obligatorio.")]
        [EmailAddress(ErrorMessage = "Email inválido.")]
        public string EmailCliente { get; set; } = string.Empty;

        [Range(0.001, 1000, ErrorMessage = "El peso no es válido.")]
        public decimal Peso { get; set; }

        [Required]
        public string Unidad { get; set; } = "kg"; // "kg" o "lb"

        [Required(ErrorMessage = "Selecciona un destino.")]
        [EnumDataType(typeof(RegionEnvio), ErrorMessage = "La región no es válida.")]
        public RegionEnvio Region { get; set; }

        public string? UserId { get; set; }
    }

    // ── Response ───────────────────────────────────────────────────────────────

    public class CalcularEnvioResponse
    {
        public int      Id             { get; set; }
        public string   NombreCliente  { get; set; } = string.Empty;
        public string   EmailCliente   { get; set; } = string.Empty;
        public decimal  PesoKg         { get; set; }
        public string   Region         { get; set; } = string.Empty;
        public decimal  TarifaPorKg    { get; set; }
        public decimal  CostoTotal     { get; set; }
        public string?  UserId         { get; set; }
        public DateTime FechaCalculo   { get; set; }
    }

    public class TarifaRegionResponse
    {
        public string   Codigo      { get; set; } = string.Empty;
        public string   NombrePais  { get; set; } = string.Empty;
        public decimal  TarifaPorKg { get; set; }
    }

    public class HistorialCalculoResponse
    {
        public int      Id            { get; set; }
        public string   NombreCliente { get; set; } = string.Empty;
        public decimal  PesoKg        { get; set; }
        public string   Region        { get; set; } = string.Empty;
        public decimal  CostoTotal    { get; set; }
        public string?  UserId        { get; set; }
        public DateTime FechaCalculo  { get; set; }
    }
}
