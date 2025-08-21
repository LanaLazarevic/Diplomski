using PFM.Domain.Dtos;
using PFM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFM.Domain.Interfaces
{
    public interface IUserRepository
    {
        void Add(User user);
        Task<User?> GetByEmailAsync(string email, CancellationToken ct = default);

        Task<User?> GetByIdAsync(Guid id, CancellationToken ct = default);

        Task<PagedList<User>> GetUsersAsync(UserQuerySpecification spec, CancellationToken ct = default);

        void Update(User user);

        Task<List<User>> GetAllWithCardsAndTransactionsAsync(DateTime start, DateTime end, CancellationToken ct = default);
    }
}
