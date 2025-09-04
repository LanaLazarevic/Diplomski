using MediatR;
using Microsoft.AspNetCore.Mvc;
using PFM.Application.Dto;
using PFM.Application.Result;
using PFM.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFM.Application.UseCases.Accounts.Queries.GetAll
{
    public class GetAccountsQuery : IRequest<OperationResult<PagedList<AccountDto>>>
    {
        [FromQuery(Name = "account-number")]
        public long? AccountNumber { get; set; }

        [FromQuery(Name = "user-jmbg")]
        public string? UserJmbg { get; set; }

        [FromQuery(Name = "page")]
        public int Page { get; set; } = 1;

        [FromQuery(Name = "page-size")]
        public int PageSize { get; set; } = 10;

        [FromQuery(Name = "sort-by")]
        public string SortBy { get; set; } = "accountNumber";

        [FromQuery(Name = "sort-order")]
        public string SortOrder { get; set; } = "Asc";

        public Guid? UserId { get; set; }
    }
}