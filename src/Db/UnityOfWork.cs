using CaseLocaliza.Db.Abstractions;
using CaseLocaliza.Db.Context;

namespace CaseLocaliza.Db;

public sealed class UnityOfWork : IUnityOfWork, IDisposable
{
    private readonly AppDbContext _context;

    public UnityOfWork(AppDbContext context) => _context = context;

    public async Task BeginTransactionAsync() => await _context.Database.BeginTransactionAsync();

    public async Task CommitAsync() => await _context.SaveChangesAsync();
        
    public async Task RollBackAsync() => await _context.Database.RollbackTransactionAsync();

    public void Dispose() => _context.Dispose();
}
