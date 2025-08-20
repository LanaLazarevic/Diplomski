using AutoMapper;
using MediatR;
using PFM.Application.Dto;
using PFM.Application.Result;
using PFM.Domain.Dtos;
using PFM.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFM.Application.UseCases.Cards.Queries.GetAll
{
    public class GetCardsQueryHandler : IRequestHandler<GetCardsQuery, OperationResult<PagedList<CardDto>>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public GetCardsQueryHandler(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<OperationResult<PagedList<CardDto>>> Handle(GetCardsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var sortEnum = Enum.Parse<SortOrder>(request.SortOrder, true);
                var spec = new CardQuerySpecification(request.OwnerName, request.Page, request.PageSize, request.SortBy, sortEnum, request.UserId);
                var cards = await _uow.Cards.GetCardsAsync(spec, cancellationToken);

                if (request.Page > cards.TotalPages && cards.TotalPages != 0)
                {
                    ValidationError error = new ValidationError
                    {
                        Tag = "page",
                        Error = "out-of-range",
                        Message = $"Page {request.Page} is out of range. Total pages: {cards.TotalPages}."
                    };
                    List<ValidationError> errors = new List<ValidationError> { error };
                    return OperationResult<PagedList<CardDto>>.Fail(440, errors);
                }

                var dtos = _mapper.Map<List<CardDto>>(cards.Items);
                var result = new PagedList<CardDto>
                {
                    Items = dtos,
                    TotalCount = cards.TotalCount,
                    PageSize = cards.PageSize,
                    Page = cards.Page,
                    SortBy = cards.SortBy,
                    SortOrderd = cards.SortOrderd,
                    TotalPages = cards.TotalPages
                };

                return OperationResult<PagedList<CardDto>>.Success(result, 200);
            }
            catch (Exception ex)
            {
                var problem = new ServerError { Message = ex.Message };
                return OperationResult<PagedList<CardDto>>.Fail(503, new[] { problem });
            }
        }
    }
}
