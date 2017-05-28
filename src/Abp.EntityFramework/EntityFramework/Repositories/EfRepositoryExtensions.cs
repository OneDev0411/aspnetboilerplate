using System;
using System.Data.Entity;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Reflection;

namespace Abp.EntityFramework.Repositories
{
    public static class EfRepositoryExtensions
    {
        public static DbContext GetDbContext<TEntity, TPrimaryKey>(this IRepository<TEntity, TPrimaryKey> repository)
            where TEntity : class, IEntity<TPrimaryKey>
        {
            var repositoryWithDbContext = repository as IRepositoryWithDbContext;
            if (repositoryWithDbContext == null)
            {
                throw new ArgumentException("Given repository does not implement IRepositoryWithDbContext", nameof(repository));
            }

            var targetWithDbContext = CastleProxyHelper.GetProxyTargetOrNull(repository) as IRepositoryWithDbContext;
            if (targetWithDbContext != null)
            {
                return targetWithDbContext.GetDbContext();
            }

            return repositoryWithDbContext.GetDbContext();
        }

        public static void DetachFromDbContext<TEntity, TPrimaryKey>(this IRepository<TEntity, TPrimaryKey> repository, TEntity entity)
            where TEntity : class, IEntity<TPrimaryKey>
        {
            repository.GetDbContext().Entry(entity).State = EntityState.Detached;
        }
    }
}