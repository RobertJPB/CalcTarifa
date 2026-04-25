using System.ComponentModel.DataAnnotations;
using CalcTarifa.Domain.Enums;

namespace CalcTarifa.Web.Models
{
    // ── Formulario ─────────────────────────────────────────────────────────────

    public class CalcularEnvioForm
    {
        [Required(ErrorMessage = "Selecciona el país de destino.")]
        [Display(Name = "País de destino")]
        public RegionEnvio? Region { get; set; }

        [Required(ErrorMessage = "Ingresa el peso del paquete.")]
        [Range(0.001, 1000, ErrorMessage = "El peso ingresado no es válido.")]
        [Display(Name = "Peso")]
        public decimal? Peso { get; set; }

        public string Unidad { get; set; } = "kg";
        public decimal? PesoIngresado { get; set; }
    }

    // ── Resultado ──────────────────────────────────────────────────────────────

    public class ResultadoEnvioVM
    {
        public string   NombreCliente { get; set; } = string.Empty;
        public decimal  PesoKg        { get; set; }
        public decimal  PesoVisual    { get; set; }
        public string   Unidad        { get; set; } = "kg";
        public string   Region        { get; set; } = string.Empty;
        public decimal  TarifaPorKg   { get; set; }
        public decimal  CostoTotal    { get; set; }
        public DateTime FechaCalculo  { get; set; }
    }

    // ── Tarifa info ────────────────────────────────────────────────────────────

    public class TarifaVM
    {
        public string      NombrePais  { get; set; } = string.Empty;
        public string      Codigo      { get; set; } = string.Empty;
        public decimal     TarifaPorKg { get; set; }
        public RegionEnvio Region      { get; set; }
    }

    // ── Historial ──────────────────────────────────────────────────────────────

    public class HistorialItemVM
    {
        public string   NombreCliente { get; set; } = string.Empty;
        public decimal  PesoKg        { get; set; }
        public string   Region        { get; set; } = string.Empty;
        public decimal  CostoTotal    { get; set; }
        public DateTime FechaCalculo  { get; set; }
    }

    // ── ViewModel raíz ────────────────────────────────────────────────────────

    public class HomeVM
    {
        public CalcularEnvioForm    Formulario     { get; set; } = new();
        public ResultadoEnvioVM?    Resultado      { get; set; }
        public List<TarifaVM>       Tarifas        { get; set; } = new();
        public List<HistorialItemVM> Historial     { get; set; } = new();
        public bool                 TieneResultado => Resultado is not null;
    }
}
