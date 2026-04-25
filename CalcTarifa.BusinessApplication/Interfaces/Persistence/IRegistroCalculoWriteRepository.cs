using CalcTarifa.Domain.Entities;

namespace CalcTarifa.BusinessApplication.Interfaces.Persistence
{
    public interface IRegistroCalculoWriteRepository
    {
        Task AgregarAsync(RegistroCalculo registro);
    }
}
