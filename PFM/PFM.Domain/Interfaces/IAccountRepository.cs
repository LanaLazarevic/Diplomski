using PFM.Domain.Dtos;
using PFM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFM.Domain.Interfaces
{
    public interface IAccountRepository
    {
        void Add(Account account);
        void Update(Account account);
        Task<Account?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<Account?> GetByNumber(long accountNumber, CancellationToken ct = default);
        Task<PagedList<Account>> GetAccountsAsync(AccountQuerySpecification spec, CancellationToken ct = default);
    }
}
