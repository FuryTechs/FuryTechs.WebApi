using System;
using System.Linq.Expressions;
using Autofac;
using FuryTechs.WebApi.Controller;
using FuryTechs.WebApi.Example.Models.Database;
using FuryTechs.WebApi.Example.Models.Dto;

namespace FuryTechs.WebApi.Example.Controllers
{
    public class UsersController : BaseApi<User, Guid, UserDto, Guid>
    {
        public UsersController(ILifetimeScope scope) : base(scope)
        {
        }

        protected override Expression<Func<User, Guid>> GetKeyValue => x => x.Id;

        protected override Expression<Func<UserDto, Guid>> GetDtoInKeyValue => x => x.UserId;
    }
}
