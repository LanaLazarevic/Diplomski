using MediatR;
using Microsoft.AspNetCore.Mvc;
using PFM.Application.Dto;
using PFM.Application.Result;
using PFM.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PFM.Application.UseCases.Cards.Queries.GetAll
{
    public class GetCardsQuery : IRequest<OperationResult<PagedList<CardDto>>>
    {
        [FromQuery(Name = "owner-name")]
        public string? OwnerName { get; set; }

        [FromQuery(Name = "page")]
        public int Page { get; set; } = 1;

        [FromQuery(Name = "page-size")]
        public int PageSize { get; set; } = 10;

        [FromQuery(Name = "sort-by")]
        public string SortBy { get; set; } = "ownerName";

        [FromQuery(Name = "sort-order")]
        public string SortOrder { get; set; } = "Asc";

        [JsonIgnore]
        public string? Email { get; set; }

    }
}
