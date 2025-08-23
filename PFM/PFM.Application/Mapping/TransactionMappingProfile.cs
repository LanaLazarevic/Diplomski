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
            CreateMap<Transaction, TransactionDto>();
            CreateMap<Split, SplitItemDto>();
            CreateMap<Category, CategoryDto>();
            CreateMap<Card, CardDto>()
                 .ForMember(dest => dest.CardType, opt => opt.MapFrom(src => src.CardType.ToString()));
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()));

        }
    }
}
