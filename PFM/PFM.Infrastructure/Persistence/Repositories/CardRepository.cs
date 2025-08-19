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
        public async Task<PagedList<Card>> GetCardsAsync(CardQuerySpecification spec, CancellationToken ct = default)
        {
            var query = _context.Cards.AsQueryable();
            if (!string.IsNullOrWhiteSpace(spec.OwnerName))
            {
                var lowered = spec.OwnerName.ToLower();
                query = query.Where(c => c.OwnerName.ToLower().Contains(lowered));
            }

            query = spec.SortBy.ToLower() switch
            {
                "cardnumber" => spec.SortOrder == SortOrder.Asc
                    ? query.OrderBy(c => c.CardNumber)
                    : query.OrderByDescending(c => c.CardNumber),
                "ownername" => spec.SortOrder == SortOrder.Asc
                    ? query.OrderBy(c => c.OwnerName)
                    : query.OrderByDescending(c => c.OwnerName),
                _ => spec.SortOrder == SortOrder.Asc
                    ? query.OrderBy(c => c.OwnerName)
                    : query.OrderByDescending(c => c.OwnerName)
            };

            var totalCount = await query.CountAsync(ct);
            var items = await query
                .Skip((spec.Page - 1) * spec.PageSize)
                .Take(spec.PageSize)
                .ToListAsync(ct);

            return new PagedList<Card>
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
    }
}
