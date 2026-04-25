namespace CalcTarifa.Domain.Enums
{
    /// <summary>
    /// Regiones soportadas para el cálculo de tarifas de envío internacional.
    /// Añadir un nuevo valor aquí es el único cambio necesario para soportar un país adicional.
    /// </summary>
    public enum RegionEnvio
    {
        India = 1,  // USD 5 / kg
        US    = 2,  // USD 8 / kg
        UK    = 3   // USD 10 / kg
    }
}
