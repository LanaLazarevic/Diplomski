using Microsoft.EntityFrameworkCore;
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
    }
}
