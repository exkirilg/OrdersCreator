namespace Domain.DataAccess;

public interface IUnitOfWork : IDisposable
{
    public IOrdersRepository OrdersRepository { get; }
    public IProvidersRepository ProvidersRepository { get; }
    Task Save();
}
