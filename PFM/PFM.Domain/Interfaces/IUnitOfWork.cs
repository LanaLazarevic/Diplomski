namespace PFM.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        ITransactionRepository Transactions { get; }
        ICategoryRepository Categories { get; }
        IUserRepository Users { get; }
        IAccountRepository Accounts { get; }
        ICardRepository Cards { get; }

        Task SaveChangesAsync();
        Task SaveChangesAsync(CancellationToken cancellationToken);
    }
}
