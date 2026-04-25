namespace CalcTarifa.Infrastructure.Configurations
{
    public class DatabaseOptions
    {
        public const string ConnectionStrings = "ConnectionStrings";
        public string DefaultConnection { get; set; } = string.Empty;
    }
}
