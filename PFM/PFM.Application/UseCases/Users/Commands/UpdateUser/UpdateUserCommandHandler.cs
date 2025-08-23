using FluentValidation;
using MediatR;
using PFM.Application.Dto;
using PFM.Application.Result;
using PFM.Domain.Enums;
using PFM.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFM.Application.UseCases.Users.Commands.UpdateUser
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, OperationResult>
    {
        private readonly IUnitOfWork _uow;
        private readonly IValidator<UpdateUserDto> _validator;

        public UpdateUserCommandHandler(IUnitOfWork uow, IValidator<UpdateUserDto> validator)
        {
            _uow = uow;
            _validator = validator;
        }

        public async Task<OperationResult> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Dto;
            var validation = await _validator.ValidateAsync(dto, cancellationToken);
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
                return OperationResult.Fail(400, errors);
            }

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
                    return OperationResult.Fail(440, new[] { error });
                }

                user.FirstName = dto.FirstName;
                user.LastName = dto.LastName;
                user.Email = dto.Email;
                user.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password);
                user.Address = dto.Address;
                user.PhoneNumber = dto.PhoneNumber;
               

                _uow.Users.Update(user);
                await _uow.SaveChangesAsync(cancellationToken);

                return OperationResult.Success();
            }
            catch (Exception ex)
            {
                var problem = new ServerError { Message = ex.Message };
                return OperationResult.Fail(503, new[] { problem });
            }
        }
    }
}
