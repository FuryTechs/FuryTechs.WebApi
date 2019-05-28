using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;
using FuryTechs.BLM.EntityFrameworkCore.Identity;
using Microsoft.AspNetCore.Http;

namespace FuryTechs.WebApi.IdentityResolver
{
    /// <summary>
    /// Identity resolver, which will resolve the user's identity
    /// </summary>
    public class IdentityResolver : IIdentityResolver
    {
        private readonly IHttpContextAccessor _contextAccessor;

        /// <summary>
        /// Creates a new instance from the IdentityResolver object which would be used by the BLM framework
        /// </summary>
        /// <param name="contextAccessor"></param>
        public IdentityResolver(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        /// <summary>
        /// Gets the user identity from the HttpContext
        /// </summary>
        /// <returns>User's identity</returns>
        public IIdentity GetIdentity()
        {
            return _contextAccessor.HttpContext.User.Identity;
        }
    }
}
