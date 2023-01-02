namespace Shared.Data.UnitOfWork
{
    public interface IUnitOfWork
    {
        Task<int> CommitAsync(CancellationToken cancellationToken = default);
        int Commit();
        void Rollback();

    }
}
