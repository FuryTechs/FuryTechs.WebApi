using System;
using System.Linq.Expressions;
using Autofac;
using FuryTechs.WebApi.Controller;
using FuryTechs.WebApi.Example.Models.Database;
using FuryTechs.WebApi.Example.Models.Dto;

namespace FuryTechs.WebApi.Example.Controllers
{
    public class MessagesController : BaseApi<Message, Guid, MessageDto, Guid>
    {
        public MessagesController(ILifetimeScope scope) : base(scope)
        {
        }

        protected override Expression<Func<Message, Guid>> GetKeyValue => x => x.Id;

        protected override Expression<Func<MessageDto, Guid>> GetDtoInKeyValue => x => x.MessageId;
    }
}
