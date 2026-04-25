using CalcTarifa.Domain.Validations;

namespace CalcTarifa.Domain.ValueObjects
{
    // representa un peso en kg validado
    public sealed class PesoKg
    {
        public decimal Valor { get; }

        private PesoKg(decimal valor) => Valor = valor;

        public static PesoKg Crear(decimal valor)
        {
            Validar(valor);
            return new PesoKg(Math.Round(valor, 3));
        }

        private static void Validar(decimal valor)
        {
            if (valor <= 0)
                throw new DomainValidationException("el peso debe ser mayor a cero");
            if (valor > 1000)
                throw new DomainValidationException("el peso no puede superar los 1000 kg");
        }

        public override bool Equals(object? obj) => obj is PesoKg other && Valor == other.Valor;
        public override int GetHashCode()         => Valor.GetHashCode();
        public override string ToString()          => $"{Valor} kg";
    }
}
