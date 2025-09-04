using AutoMapper;
using PFM.Domain.Dtos;
using PFM.Domain.Entities;
using PFM.Application.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PFM.Application.Dtos;

namespace PFM.Application.Mapping
{
    public class TransactionMappingProfile : Profile
    {
        public TransactionMappingProfile()
        {
            CreateMap<Transaction, TransactionDto>()
                .ForMember(dest => dest.CardNumber, opt => opt.MapFrom(src => src.Card.CardNumber));
            CreateMap<Split, SplitItemDto>();
            CreateMap<Category, CategoryDto>();
            CreateMap<Card, CardDto>()
                 .ForMember(dest => dest.CardType, opt => opt.MapFrom(src => src.CardType.ToString()))
                 .ForMember(dest => dest.AccountNumber, opt => opt.MapFrom(src => src.Account.AccountNumber));
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()));
            CreateMap<Account, AccountDto>()
                .ForMember(dest => dest.AccountType, opt => opt.MapFrom(src => src.AccountType.ToString()))
                .ForMember(dest => dest.UserFullName, opt => opt.MapFrom(src => src.User.FirstName + " " + src.User.LastName));
        }
    }
}
