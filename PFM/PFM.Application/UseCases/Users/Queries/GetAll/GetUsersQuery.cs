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

namespace PFM.Application.UseCases.Users.Queries.GetAll
{
    public class GetUsersQuery : IRequest<OperationResult<PagedList<UserDto>>>
    {
        [FromQuery(Name = "first-name")]
        public string? FirstName { get; set; }

        [FromQuery(Name = "last-name")]
        public string? LastName { get; set; }

        [FromQuery(Name = "page")]
        public int Page { get; set; } = 1;

        [FromQuery(Name = "page-size")]
        public int PageSize { get; set; } = 10;

        [FromQuery(Name = "sort-by")]
        public string SortBy { get; set; } = "firstName";

        [FromQuery(Name = "sort-order")]
        public string SortOrder { get; set; } = "Asc";


    }
}
