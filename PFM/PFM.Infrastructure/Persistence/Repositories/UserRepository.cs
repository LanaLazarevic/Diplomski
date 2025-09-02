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
    public class UserRepository : IUserRepository
    {
        private readonly PFMDbContext _ctx;

        public UserRepository(PFMDbContext ctx)
        {
            _ctx = ctx;
        }

        public void Add(User user)
        {
            _ctx.Users.Add(user);
        }

        public async Task<User?> GetByEmailAsync(string email, CancellationToken ct = default)
        {
            return await _ctx.Users.SingleOrDefaultAsync(u => u.Email == email, ct);
        }

        public async Task<User?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await _ctx.Users.FindAsync(new object?[] { id }, ct);
        }

        public async Task<PagedList<User>> GetUsersAsync(UserQuerySpecification spec, CancellationToken ct = default)
        {
            var query = _ctx.Users.AsQueryable();
            if (!string.IsNullOrWhiteSpace(spec.FirstName))
            {
                var lowered = spec.FirstName.ToLower();
                query = query.Where(u => u.FirstName.ToLower().Contains(lowered));
            }
            if (!string.IsNullOrWhiteSpace(spec.LastName))
            {
                var lowered = spec.LastName.ToLower();
                query = query.Where(u => u.LastName.ToLower().Contains(lowered));
            }

            query = spec.SortBy.ToLower() switch
            {
                "firstname" => spec.SortOrder == SortOrder.Asc
                    ? query.OrderBy(u => u.FirstName)
                    : query.OrderByDescending(u => u.FirstName),
                "lastname" => spec.SortOrder == SortOrder.Asc
                    ? query.OrderBy(u => u.LastName)
                    : query.OrderByDescending(u => u.LastName),
                "email" => spec.SortOrder == SortOrder.Asc
                    ? query.OrderBy(u => u.Email)
                    : query.OrderByDescending(u => u.Email),
                _ => spec.SortOrder == SortOrder.Asc
                    ? query.OrderBy(u => u.FirstName)
                    : query.OrderByDescending(u => u.FirstName)
            };

            var totalCount = await query.CountAsync(ct);
            var items = await query
                .Skip((spec.Page - 1) * spec.PageSize)
                .Take(spec.PageSize)
                .ToListAsync(ct);

            return new PagedList<User>
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

        public async Task<List<User>> GetAllWithCardsAndTransactionsAsync(DateTime start, DateTime end, CancellationToken ct = default)
        {
            return await _ctx.Users
                .Include(u => u.Cards!)
                    .ThenInclude(c => c.Transactions!.Where(t => t.Date >= start && t.Date <= end))
                .ToListAsync(ct);
        }

        public void Update(User user)
        {
            _ctx.Users.Update(user);
        }

        public async Task<User?> GetByJmbg(string jm, CancellationToken ct = default)
        {
            return await _ctx.Users.SingleOrDefaultAsync(u => u.Jmbg == jm, ct);
        }
    }
}
