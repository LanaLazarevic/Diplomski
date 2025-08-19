using PFM.Domain.Dtos;
using PFM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFM.Domain.Interfaces
{
    public interface ICardRepository
    {
        void Add(Card card);

        Task<Card?> GetByIdAsync(Guid id, CancellationToken ct = default);

        Task<PagedList<Card>> GetCardsAsync(CardQuerySpecification spec, CancellationToken ct = default);
    }
}
