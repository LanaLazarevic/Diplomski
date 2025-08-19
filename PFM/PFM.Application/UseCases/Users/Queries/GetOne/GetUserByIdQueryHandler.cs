using AutoMapper;
using MediatR;
using PFM.Application.Dto;
using PFM.Application.Result;
using PFM.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFM.Application.UseCases.Users.Queries.GetOne
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, OperationResult<UserDto>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public GetUserByIdQueryHandler(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<OperationResult<UserDto>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _uow.Users.GetByIdAsync(request.Id, cancellationToken);
                if (user == null)
                {
                    var error = new BusinessError
                    {
                        Problem = "user-id",
                        Message = "User not found",
                        Details = $"User with id {request.Id} does not exist"
                    };
                    return OperationResult<UserDto>.Fail(440, new[] { error });
                }

                var dto = _mapper.Map<UserDto>(user);

                return OperationResult<UserDto>.Success(dto, 200);
            }
            catch (Exception ex)
            {
                var problem = new ServerError { Message = ex.Message };
                return OperationResult<UserDto>.Fail(503, new[] { problem });
            }
        }
    }
}
