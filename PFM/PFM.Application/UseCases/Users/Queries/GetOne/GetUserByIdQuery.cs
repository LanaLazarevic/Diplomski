using MediatR;
using PFM.Application.Dto;
using PFM.Application.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFM.Application.UseCases.Users.Queries.GetOne
{
    public record GetUserByIdQuery(Guid Id) : IRequest<OperationResult<UserDto>>;

}
