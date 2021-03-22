using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Votacao.Repository.Models;
using Votacao.Security;
using Votacao.Security.Models;

namespace Votacao.Repository
{
    public class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        protected DbSet<TEntity> Entities { get; }
        protected ModelContext Context { get; }

        public BaseRepository(ModelContext context)
        {
            Context = context;
            Entities = Context.Set<TEntity>();
        }

        protected virtual void OnAdd(TEntity entity, IUserContext userContext)
        {
            entity.CreationDate = DateTime.Now;
            entity.CreationIp = userContext.IP;
            entity.CreationUser = userContext.Principal;

            OnUpdate(entity, userContext);
        }

        protected virtual void OnUpdate(TEntity entity, IUserContext userContext)
        {
            entity.EditionDate = DateTime.Now;
            entity.EditionIp = userContext.IP;
            entity.EditionUser = userContext.Principal;
        }

        protected virtual IQueryable<TEntity> GetEntities()
        {
            return Entities;
        }

        public async Task SaveAsync(CancellationToken ct = default)
        {
            await Context.SaveChangesAsync(ct);
        }

        public virtual async Task<IEnumerable<TEntity>> QueryAsync(Expression<Func<IQueryable<TEntity>, IQueryable<TEntity>>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, CancellationToken ct = default)
        {
            return await Task.Run(() =>
            {
                try
                {
                    var query = GetEntities();

                    if (filter == null) filter = x => x;
                    query = filter.Compile()(query);

                    if (orderBy != null) query = orderBy(query);
                    else query = query.OrderBy(x => x.Id);

                    return query.ToArray();
                }
                catch (Exception ex)
                {
                    throw new ServiceException("Erro", "Ocorreu um erro ao executar o serviço", ex);
                }
            }, ct);
        }

        public virtual async Task<TResult> QueryScalarAsync<TResult>(Expression<Func<IQueryable<TEntity>, Task<TResult>>> filter, CancellationToken ct = default)
        {
            return await Task.Run(async () =>
            {
                try
                {
                    var query = GetEntities();                    

                    var result = await filter?.Compile()(query);

                    try
                    {
                        return result;
                    }
                    catch 
                    {
                        return default;
                    }
                }
                catch (Exception ex)
                {
                    throw new ServiceException("Erro", "Ocorreu um erro ao executar o serviço", ex);
                }
            }, ct);
        }

        public virtual async Task<TEntity> QueryByIdAsync(int id, CancellationToken ct = default)
        {
            try
            {
                return await GetEntities().SingleAsync(x => x.Id == id, ct);
            }
            catch (Exception ex)
            {
                throw new ServiceException("Erro", "Ocorreu um erro ao executar o serviço", ex);
            }
        }

        public virtual async Task<TEntity> AddAsync(TEntity entity, IUserContext userContext, CancellationToken ct = default)
        {
            return await Task.Run(async () =>
            {
                try
                {
                    OnAdd(entity, userContext);

                    await Entities.AddAsync(entity, ct).ConfigureAwait(false);

                    return entity;
                }
                catch (Exception ex)
                {
                    throw new ServiceException("Erro", "Ocorreu um erro ao executar o serviço", ex);
                }
            }, ct);
        }

        public virtual async Task<IEnumerable<TEntity>> AddAllAsync(IEnumerable<TEntity> entities, IUserContext userContext, CancellationToken ct = default)
        {
            return await Task.Run(async () =>
            {
                try
                {
                    foreach (var entity in entities)
                        OnAdd(entity, userContext);

                    await Entities.AddRangeAsync(entities, ct).ConfigureAwait(false);

                    return entities;
                }
                catch (Exception ex)
                {
                    throw new ServiceException("Erro", "Ocorreu um erro ao executar o serviço", ex);
                }
            }, ct);
        }

        public virtual async Task<TEntity> UpdateAsync(TEntity entity, IUserContext userContext, CancellationToken ct = default)
        {
            return await Task.Run(() =>
            {
                try
                {
                    OnUpdate(entity, userContext);

                    Entities.Update(entity);
                    return entity;
                }
                catch (Exception ex)
                {
                    throw new ServiceException("Erro", "Ocorreu um erro ao executar o serviço", ex);
                }
            }, ct);
        }

        public virtual async Task<IEnumerable<TEntity>> UpdateAllAsync(IEnumerable<TEntity> entities, IUserContext userContext, CancellationToken ct = default)
        {
            return await Task.Run(() =>
            {
                try
                {
                    foreach (var entity in entities)
                        OnUpdate(entity, userContext);

                    Entities.UpdateRange(entities);

                    return entities;
                }
                catch (Exception ex)
                {
                    throw new ServiceException("Erro", "Ocorreu um erro ao executar o serviço", ex);
                }
            }, ct);
        }

        public virtual async Task<bool> DeleteAsync(int id, IUserContext userContext, CancellationToken ct = default)
        {
            return await Task.Run(async () =>
            {
                try
                {
                    TEntity toRemove;
                    try
                    {
                        toRemove = await Entities.SingleAsync(x => x.Id == id);
                    }
                    catch { throw new ServiceException("Não encontrado!", "Não foi possivel localizar apenas um valor que satifaz a condição"); }

                    Entities.Remove(toRemove);

                    return true;
                }
                catch (Exception ex)
                {
                    throw new ServiceException("Erro", "Ocorreu um erro ao executar o serviço", ex);
                }
            }, ct);
        }

        public virtual async Task<string> DeleteAllAsync(string ids, IUserContext userContext, CancellationToken ct = default)
        {
            return await Task.Run(async () =>
            {
                try
                {
                    var toReturn = new List<string>();

                    foreach (var sid in ids.Split(','))
                    {
                        try
                        {
                            if (!int.TryParse(sid, out int id)) throw new FormatException("Não é possivel converter os ids fornecidos em numeros");
                            var toRemove = await Entities.SingleAsync(x => x.Id == id, ct);
                            Entities.Remove(toRemove);
                        }
                        catch (FormatException ex) { throw new ServiceException("Formato incorreto!", ex.Message); }
                        catch { toReturn.Add(sid); }
                    }

                    if (toReturn.Count == 0) return null;
                    return string.Join(',', toReturn);
                }
                catch (Exception ex)
                {
                    throw new ServiceException("Erro", "Ocorreu um erro ao executar o serviço", ex);
                }                
            }, ct);
        }
    }
}
