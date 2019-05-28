using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FuryTechs.WebApi.Example.Models.Database;
using FuryTechs.WebApi.Example.Models.Dto;

namespace FuryTechs.WebApi.Example.Models.Mapping
{
    public class ApplicationProfile : Profile
    {
        public ApplicationProfile()
        {
            Configure();
        }

        private void Configure()
        {
            CreateMap<User, UserDto>(MemberList.None)
                .ForMember(x => x.UserId, opt => opt.MapFrom(e => e.Id))
                .ForMember(x => x.Name, opt => opt.MapFrom(e => $"{e.FirstName} {e.LastName}"))
                .ForMember(x => x.EmailAddress, opt => opt.MapFrom(e => e.Email))
                .ReverseMap()
                .ForMember(x => x.Id, opt => opt.MapFrom(e => e.UserId))
                .ForMember(x => x.Email, opt => opt.MapFrom(e => e.EmailAddress))
                .ForMember(x => x.FirstName, opt => opt.ConvertUsing(new FirstNameConverter(), e => e.Name))
                .ForMember(x => x.LastName, opt => opt.ConvertUsing(new LastNameConverter(), e => e.Name));
        }
    }
}