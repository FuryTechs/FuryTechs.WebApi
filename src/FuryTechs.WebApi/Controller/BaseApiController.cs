using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FuryTechs.BLM.NetStandard.Interfaces;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Mvc;

namespace FuryTech.WebApi.Controllers
{
    /// <summary>
    /// Base API Controller
    /// Some layer of abstraction which can be used to easily implement CRUD in Asp.NET Core MVC
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    /// <typeparam name="TKey">Entity's key type</typeparam>
    public abstract class BaseApiController<T, TKey> : BaseApiController<T, TKey, T, TKey>
        where T : class, new()
    {
        protected BaseApiController(IRepository<T> repository) : base(repository)
        {
        }

        protected override Expression<Func<T, TKey>> GetDtoKeyValue() => GetKeyValue();
    }

    /// <summary>
    /// Base API Controller
    /// Some layer of abstraction which can be used to easily implement CRUD in Asp.NET Core MVC
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    /// <typeparam name="TKey">Entity's key type</typeparam>
    /// <typeparam name="TDto">Data transfer object</typeparam>
    /// <typeparam name="TDtoKey">DTO's key type</typeparam>
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseApiController<T, TKey, TDto, TDtoKey> : ControllerBase
            where T : class, new()
            where TDto : class, new()
    {
        protected IRepository<T> Repository { get; set; }

        /// <summary>
        /// Initializes the Base API controller
        /// </summary>
        /// <param name="repository">Repository which can handle the T entity type</param>
        public BaseApiController(IRepository<T> repository)
        {
            Repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        /// <summary>
        /// Helper method to retrieve the T type's key field value <see cref="TKey" />
        /// This can be send to SQL level, so your queries would be more optimised.
        /// </summary>
        /// <returns>The value of the Key field</returns>
        protected abstract Expression<Func<T, TKey>> GetKeyValue();

        /// <summary>
        /// Helper method to retrieve the TDto type's key field value <see cref="TDtoKey" />
        /// This can be send to SQL level, so your queries would be more optimised.
        /// </summary>
        /// <returns>The value of the Key field</returns>
        protected abstract Expression<Func<TDto, TDtoKey>> GetDtoKeyValue();

        [HttpGet]
        [EnableQuery]
        public async Task<IActionResult> Get(ODataQueryOptions<TDto> query)
        {

            return Ok(query.ApplyTo(Repository.Entities()));
        }

    }
}
