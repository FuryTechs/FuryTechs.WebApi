using System;
using System.Linq.Expressions;
using Autofac;
using FuryTechs.WebApi.Controller;
using FuryTechs.WebApi.Example.Models.Database;
using FuryTechs.WebApi.Example.Models.Dto;

namespace FuryTechs.WebApi.Example.Controllers
{
    public class ValuesController : BaseApi<User, int, UserDto, int>
    {
        public ValuesController(ILifetimeScope scope) : base(scope)
        {
        }

        protected override Expression<Func<User, int>> GetKeyValue => x => x.Id;

        protected override Expression<Func<UserDto, int>> GetDtoInKeyValue => x => x.UserId;
    }
}
