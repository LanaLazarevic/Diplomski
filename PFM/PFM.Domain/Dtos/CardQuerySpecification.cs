using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFM.Domain.Dtos
{
    public class CardQuerySpecification
    {
        public string? OwnerName { get; }
        public int Page { get; }
        public int PageSize { get; }
        public string SortBy { get; }
        public SortOrder SortOrder { get; }

        public CardQuerySpecification(string? ownerName, int page, int pageSize, string sortBy, SortOrder sortOrder)
        {
            OwnerName = ownerName;
            Page = page <= 0 ? 1 : page;
            PageSize = pageSize <= 0 ? 10 : pageSize;
            SortBy = !string.IsNullOrWhiteSpace(sortBy) ? sortBy : "ownername";
            SortOrder = sortOrder;
        }
    }
}
