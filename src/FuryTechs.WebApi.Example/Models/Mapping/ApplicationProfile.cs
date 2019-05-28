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
                .ForMember(x => x.Outbox, opt => opt.MapFrom(e => e.SentMessages))
                .ReverseMap()
                .ForMember(x => x.Id, opt => opt.MapFrom(e => e.UserId))
                .ForMember(x => x.Email, opt => opt.MapFrom(e => e.EmailAddress))
                .ForMember(x => x.SentMessages, opt => opt.MapFrom(e => e.Outbox))
                .ForMember(x => x.FirstName, opt => opt.ConvertUsing(new FirstNameConverter(), e => e.Name))
                .ForMember(x => x.LastName, opt => opt.ConvertUsing(new LastNameConverter(), e => e.Name))
                ;

            CreateMap<Message, MessageDto>(MemberList.None)
                .ForMember(x => x.MessageId, opt => opt.MapFrom(e => e.Id))
                .ForMember(x => x.Subject, opt => opt.MapFrom(e => e.Subject))
                .ForMember(x => x.Text, opt => opt.MapFrom(e => e.Text))
                .ForMember(x => x.CreatedAt, opt => opt.MapFrom(e => e.CreatedAt))
                .ForMember(x => x.Sender, opt => { opt.MapFrom(e => e.Sender); })
                .ReverseMap()
                .ForMember(x => x.Id, opt => opt.MapFrom(e => e.MessageId))
                .ForMember(x => x.Subject, opt => opt.MapFrom(e => e.Subject))
                .ForMember(x => x.Text, opt => opt.MapFrom(e => e.Text))
                .ForMember(x => x.CreatedAt, opt => opt.MapFrom(e => e.CreatedAt))
                .ForMember(x => x.Sender, opt => opt.MapFrom(e => e.Sender))
                ;
        }
    }
}