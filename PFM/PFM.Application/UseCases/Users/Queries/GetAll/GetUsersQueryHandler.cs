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

namespace PFM.Application.UseCases.Users.Queries.GetAll
{
    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, OperationResult<PagedList<UserDto>>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public GetUsersQueryHandler(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<OperationResult<PagedList<UserDto>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var sortEnum = Enum.Parse<SortOrder>(request.SortOrder, true);
                var spec = new UserQuerySpecification(request.FirstName, request.LastName, request.Page, request.PageSize, request.SortBy, sortEnum);
                var users = await _uow.Users.GetUsersAsync(spec, cancellationToken);

                if (request.Page > users.TotalPages && users.TotalPages != 0)
                {
                    ValidationError error = new ValidationError
                    {
                        Tag = "page",
                        Error = "out-of-range",
                        Message = $"Page {request.Page} is out of range. Total pages: {users.TotalPages}."
                    };
                    List<ValidationError> errors = new List<ValidationError> { error };
                    return OperationResult<PagedList<UserDto>>.Fail(440, errors);
                }

                var dtos = _mapper.Map<List<UserDto>>(users.Items);
                var result = new PagedList<UserDto>
                {
                    Items = dtos,
                    TotalCount = users.TotalCount,
                    PageSize = users.PageSize,
                    Page = users.Page,
                    SortBy = users.SortBy,
                    SortOrderd = users.SortOrderd,
                    TotalPages = users.TotalPages
                };

                return OperationResult<PagedList<UserDto>>.Success(result, 200);
            }
            catch (Exception ex)
            {
                var problem = new ServerError { Message = ex.Message };
                return OperationResult<PagedList<UserDto>>.Fail(503, new[] { problem });
            }
        }
    }
}
