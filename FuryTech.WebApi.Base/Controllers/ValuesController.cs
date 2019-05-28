using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FuryTech.WebApi.Base.Db.Model;
using FuryTech.WebApi.Controllers;
using FuryTechs.BLM.NetStandard.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FuryTech.WebApi.Base.Controllers
{
    public class ValuesController : BaseApiController<User, int>
    {
        public ValuesController(IRepository<User> repository) : base(repository)
        {
        }

        protected override Expression<Func<User, int>> GetKeyValue() => x => x.Id;
    }
}
