namespace CalcTarifa.Domain.Validations
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(string message) : base(message) { }
        public EntityNotFoundException(string entityName, object key) 
            : base($"La entidad '{entityName}' con clave '{key}' no fue encontrada.") { }
    }
}
