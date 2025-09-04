using AutoMapper;
using FluentValidation;
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

namespace PFM.Application.UseCases.Accounts.Queries.GetAll
{
    public class GetAccountsQueryHandler : IRequestHandler<GetAccountsQuery, OperationResult<PagedList<AccountDto>>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly IValidator<GetAccountsQuery> _validator;

        public GetAccountsQueryHandler(IUnitOfWork uow, IMapper mapper, IValidator<GetAccountsQuery> validator)
        {
            _uow = uow;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<OperationResult<PagedList<AccountDto>>> Handle(GetAccountsQuery request, CancellationToken cancellationToken)
        {
            var validation = await _validator.ValidateAsync(request, cancellationToken);
            if (!validation.IsValid)
            {
                var errors = validation.Errors.Select(e =>
                {
                    var raw = e.ErrorMessage ?? string.Empty;
                    var parts = raw.Split(':');
                    var tag = parts.ElementAtOrDefault(0) ?? e.PropertyName;
                    var code = parts.ElementAtOrDefault(1) ?? e.ErrorCode;
                    var message = parts.ElementAtOrDefault(2) ?? raw;
                    return new ValidationError
                    {
                        Tag = tag,
                        Error = code,
                        Message = message
                    };
                }).ToList();
                return OperationResult<PagedList<AccountDto>>.Fail(400, errors);
            }

            try
            {
                var sortEnum = Enum.Parse<SortOrder>(request.SortOrder, true);
                var spec = new AccountQuerySpecification(request.AccountNumber, request.UserJmbg, request.Page, request.PageSize, request.SortBy, sortEnum, request.UserId);
                var accounts = await _uow.Accounts.GetAccountsAsync(spec, cancellationToken);

                if (request.Page > accounts.TotalPages && accounts.TotalPages != 0)
                {
                    ValidationError error = new ValidationError
                    {
                        Tag = "page",
                        Error = "out-of-range",
                        Message = $"Page {request.Page} is out of range. Total pages: {accounts.TotalPages}."
                    };
                    List<ValidationError> errors = new List<ValidationError> { error };
                    return OperationResult<PagedList<AccountDto>>.Fail(440, errors);
                }

                var dtos = _mapper.Map<List<AccountDto>>(accounts.Items);
                var result = new PagedList<AccountDto>
                {
                    Items = dtos,
                    TotalCount = accounts.TotalCount,
                    PageSize = accounts.PageSize,
                    Page = accounts.Page,
                    SortBy = accounts.SortBy,
                    SortOrderd = accounts.SortOrderd,
                    TotalPages = accounts.TotalPages
                };

                return OperationResult<PagedList<AccountDto>>.Success(result, 200);
            }
            catch (Exception ex)
            {
                var problem = new ServerError { Message = ex.Message };
                return OperationResult<PagedList<AccountDto>>.Fail(503, new[] { problem });
            }
        }
    }
}
