using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFM.Domain.Dtos
{
    public class AccountQuerySpecification
    {
        public long? AccountNumber { get; }
        public string? Jmbg { get; }
        public int Page { get; }
        public int PageSize { get; }
        public string SortBy { get; }
        public SortOrder SortOrder { get; }
        public Guid? UserId { get; }

        public AccountQuerySpecification(long? accountNumber, string? jmbg, int page, int pageSize, string sortBy, SortOrder sortOrder, Guid? userId)
        {
            AccountNumber = accountNumber;
            Jmbg = jmbg;
            Page = page <= 0 ? 1 : page;
            PageSize = pageSize <= 0 ? 10 : pageSize;
            SortBy = !string.IsNullOrWhiteSpace(sortBy) ? sortBy : "accountnumber";
            SortOrder = sortOrder;
            UserId = userId;
        }
    }
}
