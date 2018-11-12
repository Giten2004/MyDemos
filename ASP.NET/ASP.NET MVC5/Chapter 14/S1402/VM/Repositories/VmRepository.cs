using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Data;
using System.Collections.Generic;
using Microsoft.Practices.Unity.Utility;
using VM;

namespace VM.Repositories
{
    public class VmRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        public VmDbContext DbContext { get; private set; }
        public DbSet<TEntity> DbSet { get; private set; }
        public VmRepository(VmDbContext context)
        {
            Guard.ArgumentNotNull(context, "context");
            this.DbContext = context;
            this.DbSet = this.DbContext.Set<TEntity>();
        }

        public IEnumerable<TEntity> Get()
        {
            return this.DbSet.AsQueryable();
        }
        public IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter)
        {
            return this.DbSet.Where(filter).AsQueryable();
        }
        public IEnumerable<TEntity> Get<TKey>(Expression<Func<TEntity, bool>> filter, int pageIndex, int pageSize, Expression<Func<TEntity, TKey>> sortKeySelector, bool isAsc = true)
        {
            Guard.ArgumentNotNull(filter, "predicate");
            Guard.ArgumentNotNull(sortKeySelector, "sortKeySelector");
            if (isAsc)
            {
                return this.DbSet
                    .Where(filter)
                    .OrderBy(sortKeySelector)
                    .Skip(pageSize * (pageIndex - 1))
                    .Take(pageSize).AsQueryable();
            }
            else
            {
                return this.DbSet
                    .Where(filter)
                    .OrderByDescending(sortKeySelector)
                    .Skip(pageSize * (pageIndex - 1))
                    .Take(pageSize).AsQueryable();
            }
        }
        
        public int Count(Expression<Func<TEntity, bool>> predicate)
        {
            return this.DbSet.Where(predicate).Count();
        }

        public void Add(TEntity instance)
        {
            Guard.ArgumentNotNull(instance, "instance");
            this.DbSet.Attach(instance);
            this.DbContext.Entry(instance).State = EntityState.Added;
            this.DbContext.SaveChanges();
        }
        public void Update(TEntity instance)
        {
            Guard.ArgumentNotNull(instance, "instance");
            this.DbSet.Attach(instance);
            this.DbContext.Entry(instance).State = EntityState.Modified;
            this.DbContext.SaveChanges();
        }
        public void Delete(TEntity instance)
        {
            Guard.ArgumentNotNull(instance, "instance");
            this.DbSet.Attach(instance);
            this.DbContext.Entry(instance).State = EntityState.Deleted;
            this.DbContext.SaveChanges();
        }

        public void Dispose()
        {
            this.DbContext.Dispose();
        }
    }
}