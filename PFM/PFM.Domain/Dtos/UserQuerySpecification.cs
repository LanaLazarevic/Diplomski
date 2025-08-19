using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFM.Domain.Dtos
{
    public class UserQuerySpecification
    {
        public string? FirstName { get; }
        public string? LastName { get; }
        public int Page { get; }
        public int PageSize { get; }
        public string SortBy { get; }
        public SortOrder SortOrder { get; }

        public UserQuerySpecification(string? firstName, string? lastName, int page, int pageSize, string sortBy, SortOrder sortOrder)
        {
            FirstName = firstName;
            LastName = lastName;
            Page = page <= 0 ? 1 : page;
            PageSize = pageSize <= 0 ? 10 : pageSize;
            SortBy = !string.IsNullOrWhiteSpace(sortBy) ? sortBy : "firstname";
            SortOrder = sortOrder;
        }
    }
}
