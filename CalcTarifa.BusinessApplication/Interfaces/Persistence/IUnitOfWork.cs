namespace CalcTarifa.BusinessApplication.Interfaces.Persistence
{
    public interface IUnitOfWork : IDisposable, IAsyncDisposable
    {
        ITarifaRepository              Tarifas        { get; }
        IRegistroCalculoWriteRepository RegistrosWrite { get; }
        IRegistroCalculoReadRepository  RegistrosRead  { get; }
        IUsuarioRepository             Usuarios       { get; }
        Task<int>                      CommitAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
