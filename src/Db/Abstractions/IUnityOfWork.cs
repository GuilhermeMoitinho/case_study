namespace CaseLocaliza.Db.Abstractions;

public interface IUnityOfWork
{
    Task BeginTransactionAsync();

    Task CommitAsync();

    Task RollBackAsync();
}
