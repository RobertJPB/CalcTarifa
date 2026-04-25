namespace CalcTarifa.Domain.ValueObjects
{
    // representa un peso en libras validado
    public sealed class PesoLb
    {
        public decimal Valor { get; }
        public const decimal LB_TO_KG = 0.453592m;

        private PesoLb(decimal valor)
        {
            if (valor <= 0)
                throw new DomainValidationException("el peso en libras debe ser mayor a cero");
            if (valor > 2204.62m)
                throw new DomainValidationException("el peso en libras no puede superar las 2,204.62 lb (1,000 kg)");

            Valor = valor;
        }

        public static PesoLb Crear(decimal valor) => new(valor);

        // convierte a pesokg
        public PesoKg ToKg() => PesoKg.Crear(Math.Round(Valor * LB_TO_KG, 4));
    }
}
