using PFM.Domain.Interfaces;
using PFM.Infrastructure.Persistence.DbContexts;
using PFM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFM.Infrastructure.Persistence.Repositories
{
    public class CardRepository : ICardRepository
    {
        private readonly PFMDbContext _context;
        public CardRepository(PFMDbContext context)
        {
            _context = context;
        }
        public void Add(Card card)
        {
            _context.Cards.Add(card);
        }
        public async Task<Card?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await _context.Cards.FindAsync(new object?[] { id }, ct);
        }
    }
}
