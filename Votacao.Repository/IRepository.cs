using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Votacao.Repository.Models;
using Votacao.Security.Models;

namespace Votacao.Repository
{
    public interface IRepository<TEntity> where TEntity : class, IEntity
    {
        Task<IEnumerable<TEntity>> QueryAsync(Expression<Func<IQueryable<TEntity>, IQueryable<TEntity>>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, CancellationToken ct = default);
        Task<TEntity> QueryByIdAsync(int id, CancellationToken ct = default);
        Task<TResult> QueryScalarAsync<TResult>(Expression<Func<IQueryable<TEntity>, Task<TResult>>> filter, CancellationToken ct = default);
        Task<TEntity> AddAsync(TEntity entity, IUserContext userContext, CancellationToken ct = default);
        Task<IEnumerable<TEntity>> AddAllAsync(IEnumerable<TEntity> entities, IUserContext userContext, CancellationToken ct = default);
        Task<TEntity> UpdateAsync(TEntity entity, IUserContext userContext, CancellationToken ct = default);
        Task<IEnumerable<TEntity>> UpdateAllAsync(IEnumerable<TEntity> entities, IUserContext userContext, CancellationToken ct = default);
        Task<bool> DeleteAsync(int id, IUserContext userContext, CancellationToken ct = default);
        Task<string> DeleteAllAsync(string ids, IUserContext userContext, CancellationToken ct = default);
        Task SaveAsync(CancellationToken ct = default);
    }
}
