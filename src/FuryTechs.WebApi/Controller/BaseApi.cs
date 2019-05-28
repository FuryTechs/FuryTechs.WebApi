using System;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using Autofac;
using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using AutoMapper.QueryableExtensions;
using FuryTechs.BLM.EntityFrameworkCore;
using FuryTechs.BLM.NetStandard.Exceptions;
using FuryTechs.BLM.NetStandard.Interfaces;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

// ReSharper disable VirtualMemberCallInConstructor
namespace FuryTechs.WebApi.Controller
{
    /// <summary>
    /// Base API Controller
    /// Some layer of abstraction which can be used to easily implement CRUD in Asp.NET Core MVC
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    /// <typeparam name="TKey">Entity's key type</typeparam>
    public abstract class BaseApi<T, TKey> : BaseApi<T, TKey, T, TKey>
        where T : class, new()
    {
        /// <summary>
        /// Initializes the base controller
        /// </summary>
        /// <param name="lifetimeScope"></param>
        protected BaseApi(ILifetimeScope lifetimeScope) : base(lifetimeScope)
        {
        }

        protected override Expression<Func<T, TKey>> GetDtoInKeyValue => GetKeyValue;
    }

    /// <summary>
    /// Base API Controller
    /// Some layer of abstraction which can be used to easily implement CRUD in Asp.NET Core MVC
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    /// <typeparam name="TKey">Entity's key type</typeparam>
    /// <typeparam name="TDto">Data transfer object</typeparam>
    /// <typeparam name="TDtoKey">Data transfer object's key type</typeparam>
    public abstract class BaseApi<T, TKey, TDto, TDtoKey>
        : BaseApi<T, TKey, TDto, TDtoKey, TDto, TDtoKey>
        where T : class, new()
        where TDto : class, new()
    {
        /// <summary>
        /// Initializes the base controller
        /// </summary>
        /// <param name="lifetimeScope"></param>
        protected BaseApi(ILifetimeScope lifetimeScope) : base(lifetimeScope)
        {
        }

        protected override Expression<Func<TDto, TDtoKey>> GetDtoOutKeyValue => GetDtoInKeyValue;
    }

    public abstract class BaseApi<TEntity, TKey, TDtoIn, TDtoInKey, TDtoOut, TDtoOutKey> : ODataController, IDisposable
        where TEntity : class, new()
        where TDtoIn : class, new()
    {
        /// <summary>
        /// Initializes the base controller
        /// </summary>
        /// <param name="lifetimeScope"></param>
        protected BaseApi(ILifetimeScope lifetimeScope)
        {
            UpdateScope(lifetimeScope);
        }


        protected IRepository<TEntity> Repository { get; set; }

        #region Entity & Dto key getters

        private static readonly object CompileLock = new object();

        /// <summary>
        /// Helper method to retrieve the T type's key field value <see cref="TKey" />
        /// This can be send to SQL level, so your queries would be more optimized.
        /// </summary>
        /// <returns>An expression which can be used to retrieve the value of the Key field</returns>
        protected abstract Expression<Func<TEntity, TKey>> GetKeyValue { get; }

        /// <summary>
        /// Private store of the compiled expression
        /// </summary>
        private Func<TEntity, TKey> _getKeyFunc;

        /// <summary>
        /// GetKey function what can be used for EntityFrameworkCore
        /// </summary>
        protected Func<TEntity, TKey> GetKey
        {
            get
            {
                if (_getKeyFunc == null)
                {
                    lock (CompileLock)
                    {
                        if (_getKeyFunc == null)
                        {
                            _getKeyFunc = GetKeyValue.Compile();
                        }
                    }
                }

                return _getKeyFunc;
            }
        }

        /// <summary>
        /// Helper method to retrieve the TDto type's key field value <see cref="TDtoInKey" />
        /// This can be send to SQL level, so your queries would be more optimized.
        /// </summary>
        /// <returns>An expression which can be used to retrieve the value of the Key field</returns>
        protected abstract Expression<Func<TDtoIn, TDtoInKey>> GetDtoInKeyValue { get; }

        /// <summary>
        /// Private store of the compiled expression
        /// </summary>
        private Func<TDtoIn, TDtoInKey> _getDtoInKeyFunc;

        /// <summary>
        /// GetKey function what can be used for EntityFrameworkCore
        /// </summary>
        protected Func<TDtoIn, TDtoInKey> GetDtoInKey
        {
            get
            {
                if (_getDtoInKeyFunc == null)
                {
                    lock (CompileLock)
                    {
                        if (_getDtoInKeyFunc == null)
                        {
                            _getDtoInKeyFunc = GetDtoInKeyValue.Compile();
                        }
                    }
                }

                return _getDtoInKeyFunc;
            }
        }

        /// <summary>
        /// Helper method to retrieve the TDto type's key field value <see cref="TDtoOutKey" />
        /// This can be send to SQL level, so your queries would be more optimized.
        /// </summary>
        /// <returns>An expression which can be used to retrieve the value of the Key field</returns>
        protected abstract Expression<Func<TDtoOut, TDtoOutKey>> GetDtoOutKeyValue { get; }

        /// <summary>
        /// Private store of the compiled expression
        /// </summary>
        private Func<TDtoOut, TDtoOutKey> _getDtoOutKeyFunc;

        /// <summary>
        /// GetKey function what can be used for EntityFrameworkCore
        /// </summary>
        protected Func<TDtoOut, TDtoOutKey> GetDtoOutKey
        {
            get
            {
                if (_getDtoOutKeyFunc == null)
                {
                    lock (CompileLock)
                    {
                        if (_getDtoOutKeyFunc == null)
                        {
                            _getDtoOutKeyFunc = GetDtoOutKeyValue.Compile();
                        }
                    }
                }

                return _getDtoOutKeyFunc;
            }
        }

        #endregion


        /// <summary>
        /// DI lifetime scope
        /// </summary>
        protected ILifetimeScope LifetimeScope { get; private set; }

        protected IMapper Mapper { get; private set; }

        /// <summary>
        /// UpdateAsync DI lifetimeScope, by resolving new EfRepository
        /// </summary>
        /// <param name="lifetimeScope"></param>
        protected virtual void UpdateScope(ILifetimeScope lifetimeScope)
        {
            if (LifetimeScope == null)
            {
                LifetimeScope = lifetimeScope;
            }

            Repository = lifetimeScope.Resolve<IRepository<TEntity>>();
            Mapper = lifetimeScope.Resolve<IMapper>();
        }

        #region CRUD operations

        #region List

        /// <summary>
        /// Gets an dto list of DTO entities
        /// </summary>
        /// <returns>An dto if exists</returns>
        [EnableQuery]
        [Produces("application/json")]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        [ProducesResponseType((int) HttpStatusCode.Unauthorized)]
        public virtual async Task<IActionResult> Get()
        {
            var authorizedResult = await Repository.EntitiesAsync();
            //var partialResult = .UseAsDataSource().For<TDtoOut>();
            return Ok(Mapper.ProjectTo<TDtoIn>(authorizedResult));
        }

        #endregion

        #region Get

        /// <summary>
        /// Gets an dto by looking up the data transfer object's key
        /// </summary>
        /// <param name="key"></param>
        /// <response code="404">Entity not found</response>
        /// <response code="200">Entity <see>
        ///         <cref>TDtoOut</cref>
        ///     </see>
        /// </response>
        [EnableQuery]
        [Produces("application/json")]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        [ProducesResponseType((int) HttpStatusCode.Unauthorized)]
        public virtual async Task<IActionResult> Get(TDtoInKey key)
        {
            if (await ExistsAsync(key))
            {
                var partialResult = await FindByKeyAsync(key);

                return Ok(partialResult.UseAsDataSource().For<TDtoOut>());
            }

            return NotFound();
        }

        #endregion

        #region Create

        /// <summary>
        /// Creates a new instance from the data transfer object
        /// by converting it to the corresponding Entity type
        /// </summary>
        /// <param name="entityDto"></param>
        /// <response code="200">Returns the entity (and updates also)</response>
        [Produces("application/json")]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        [ProducesResponseType((int) HttpStatusCode.Unauthorized)]
        public virtual async Task<IActionResult> Post([FromBody] TDtoIn entityDto)
        {
            try
            {
                var entity = Mapper.Map<TDtoIn, TEntity>(entityDto);
                await Repository.AddAsync(entity);
                await Repository.SaveChangesAsync();

                return Ok(Mapper.Map<TEntity, TDtoOut>(entity));
            }
            catch (AuthorizationFailedException)
            {
                return Unauthorized();
            }
            catch (AutoMapperMappingException)
            {
                return UnprocessableEntity();
            }
            catch (DbException)
            {
                return StatusCode((int) HttpStatusCode.InternalServerError);
            }
        }

        #endregion

        #region Update

        /// <summary>
        /// Updates an dto by looking up by it's data transfer object key
        /// </summary>
        /// <param name="key">DTO key</param>
        /// <param name="dto">Data transfer object</param>
        /// <response code="400">The posted entity was invalid (missing field?)</response>
        /// <response code="404">The entity was not found</response>
        /// <response code="200">The update was successful</response>
        [Produces("application/json")]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        [ProducesResponseType((int) HttpStatusCode.Unauthorized)]
        public virtual async Task<IActionResult> Put(TDtoInKey key, [FromBody] TDtoIn dto)
        {
            var entity = Mapper.Map<TDtoIn, TEntity>(dto);
            if (!ModelState.IsValid || !Equals(key, GetKey(entity)))
            {
                return BadRequest();
            }

            if (!await ExistsAsync(key))
            {
                return NotFound();
            }

            ((EfRepository<TEntity>) Repository).SetEntityState(entity, EntityState.Modified);

            await Repository.SaveChangesAsync();

            return Updated(Mapper.Map<TDtoOut>(entity));
        }

        #endregion

        #region Delete

        /// <summary>
        /// Deletes an dto by looking up by it's key
        /// </summary>
        /// <param name="key">DTO key</param>
        /// <response code="200">The delete was successful</response>
        /// <response code="404">The entity was not found, or the user has no right to see it</response>
        [HttpDelete("{key}")]
        [Produces("application/json")]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        [ProducesResponseType((int) HttpStatusCode.Unauthorized)]
        public virtual async Task<IActionResult> Delete(TDtoInKey key)
        {
            if (!await ExistsAsync(key))
            {
                return NotFound();
            }

            var entity = (await FindByKeyAsync(key)).First();

            await Repository.RemoveAsync(entity);
            await Repository.SaveChangesAsync();

            return NoContent();
        }

        #endregion

        #endregion

        /// <summary>
        /// Checks whether the dto exists in the Db or not
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected virtual async Task<bool> ExistsAsync(TDtoInKey key)
        {
            var entities = await Repository.EntitiesAsync();
            var subEntities = entities.Where(GenerateExpression(key));
            return subEntities.Any();
        }

        /// <summary>
        /// Looks up for the Entity by it's key
        /// </summary>
        /// <param name="key">DTO key of dto</param>
        /// <returns>Entity</returns>
        protected virtual async Task<IQueryable<TEntity>> FindByKeyAsync(TDtoInKey key)
        {
            var entities = await Repository.EntitiesAsync();

            var entity = entities.Where(GenerateExpression(key));
            return entity;
        }


        /// <summary>
        /// Generates an expression for EntityFrameworkCore to be able to lookup for the Entity itself by a key
        /// </summary>
        /// <param name="key">DTO key</param>
        /// <returns>Expression for EFCore</returns>
        private Expression<Func<TEntity, bool>> GenerateExpression(TDtoInKey key)
        {
            var paramExpr = Expression.Parameter(typeof(TEntity), "entity");
            var guidExpr = Expression.Constant(key);
            var propAcc = Expression.MakeMemberAccess(paramExpr, GetKeyValue.GetPropertyAccess());
            var body = Expression.Equal(propAcc, guidExpr);
            return Expression.Lambda<Func<TEntity, bool>>(body, paramExpr);
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            Repository?.Dispose();
            LifetimeScope?.Dispose();
        }
    }
}