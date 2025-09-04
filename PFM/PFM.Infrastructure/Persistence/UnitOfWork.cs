

using PFM.Domain.Interfaces;
using PFM.Infrastructure.Persistence.DbContexts;

namespace PFM.Infrastructure.Persistence;


    public class UnitOfWork : IUnitOfWork
    {
        private readonly PFMDbContext _context;
        private readonly ITransactionRepository _txRepo;
        private readonly ICategoryRepository _catRepo;
        private readonly IUserRepository _userRepo;
        private readonly ICardRepository _cardRepo;
        private readonly IAccountRepository _accountRepo;

        public ITransactionRepository Transactions => _txRepo;
        public ICategoryRepository Categories => _catRepo;
        public IUserRepository Users => _userRepo;
        public ICardRepository Cards => _cardRepo;
        public IAccountRepository Accounts => _accountRepo;

        public UnitOfWork(PFMDbContext context, ITransactionRepository txRepo, ICategoryRepository catRepo, IUserRepository userRepo, ICardRepository cardRepo, IAccountRepository accRepo)
        {
                _context = context;
                _txRepo = txRepo;
                _catRepo = catRepo;
                _userRepo = userRepo;
                _cardRepo = cardRepo;
                _accountRepo = accRepo;
    }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
