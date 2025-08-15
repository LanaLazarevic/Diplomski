using FluentValidation;
using MediatR;
using PFM.Application.Dto;
using PFM.Application.Result;
using PFM.Domain.Entities;
using PFM.Domain.Enums;
using PFM.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFM.Application.UseCases.Users.Commands.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, OperationResult>
    {
        private readonly IUnitOfWork _uow;
        private readonly IValidator<CreateUserDto> _validator;

        public CreateUserCommandHandler(IUnitOfWork uow, IValidator<CreateUserDto> validator)
        {
            _uow = uow;
            _validator = validator;
        }

        public async Task<OperationResult> Handle(CreateUserCommand request, CancellationToken cancellationToken)
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
               
                var role = Enum.Parse<RoleEnum>(dto.Role, true);

                var user = new User
                {
                    Id = Guid.NewGuid(),
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Email = dto.Email,
                    Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                    Address = dto.Address,
                    PhoneNumber = dto.PhoneNumber,
                    Birthday = dto.Birthday,
                    Role = role,
                    Jmbg = dto.Jmbg
                };

                _uow.Users.Add(user);
                await _uow.SaveChangesAsync(cancellationToken);

                return OperationResult.Success();
            }
            catch (Exception ex)
            {
                var problem = new ServerError { Message = ex.Message };
                List<ServerError> errors = new() { problem };
                return OperationResult.Fail(503, errors);
            }
        }
    }
}
