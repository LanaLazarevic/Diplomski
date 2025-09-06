using Microsoft.EntityFrameworkCore;
using PFM.Domain.Dtos;
using PFM.Domain.Entities;
using PFM.Domain.Interfaces;
using PFM.Infrastructure.Persistence.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFM.Infrastructure.Persistence.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly PFMDbContext _ctx;

        public AccountRepository(PFMDbContext ctx)
        {
            _ctx = ctx;
        }

        public void Add(Account account)
        {
            _ctx.Accounts.Add(account);
        }

        public async Task<Account?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await _ctx.Accounts.FindAsync(new object?[] { id }, ct);
        }

        public void Update(Account account)
        {
            _ctx.Accounts.Update(account);
        }

        public async Task<PagedList<Account>> GetAccountsAsync(AccountQuerySpecification spec, CancellationToken ct = default)
        {
            var query = _ctx.Accounts.Include(a => a.User).AsQueryable();

            if (spec.AccountNumber.HasValue)
            {
                query = query.Where(a => a.AccountNumber == spec.AccountNumber.Value);
            }

            if (!string.IsNullOrWhiteSpace(spec.Jmbg))
            {
                query = query.Where(a => a.User.Jmbg == spec.Jmbg);
            }

            if (spec.UserId.HasValue)
            {
                query = query.Where(a => a.UserId == spec.UserId.Value);
            }

            query = spec.SortBy.ToLower() switch
            {
                "accountnumber" => spec.SortOrder == SortOrder.Asc
                    ? query.OrderBy(a => a.AccountNumber)
                    : query.OrderByDescending(a => a.AccountNumber),
                "availableamount" => spec.SortOrder == SortOrder.Asc
                    ? query.OrderBy(a => a.AvailableAmount)
                    : query.OrderByDescending(a => a.AvailableAmount),
                "reservedamount" => spec.SortOrder == SortOrder.Asc
                    ? query.OrderBy(a => a.ReservedAmount)
                    : query.OrderByDescending(a => a.ReservedAmount),
                _ => spec.SortOrder == SortOrder.Asc
                    ? query.OrderBy(a => a.AccountNumber)
                    : query.OrderByDescending(a => a.AccountNumber)
            };

            var totalCount = await query.CountAsync(ct);
            var items = await query
                .Skip((spec.Page - 1) * spec.PageSize)
                .Take(spec.PageSize)
                .ToListAsync(ct);

            return new PagedList<Account>
            {
                Items = items,
                TotalCount = totalCount,
                PageSize = spec.PageSize,
                Page = spec.Page,
                SortBy = spec.SortBy,
                SortOrderd = spec.SortOrder.ToString(),
                TotalPages = (int)Math.Ceiling((double)totalCount / spec.PageSize)
            };
        }

        public async Task<Account?> GetByNumber(long accountNumber, CancellationToken ct = default)
        {
            return await _ctx.Accounts.FirstOrDefaultAsync(a => a.AccountNumber == accountNumber, ct);
        }
    }
}
