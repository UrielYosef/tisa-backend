using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TisaBackend.Domain.Interfaces;

namespace TisaBackend.DAL.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly IServiceScopeFactory ServiceScopeFactory;

        public Repository(IServiceScopeFactory serviceScopeFactory)
        {
            ServiceScopeFactory = serviceScopeFactory;
        }

        public async Task AddAsync(T entity)
        {
            using var scope = ServiceScopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<TisaContext>(); 
            await context.Set<T>().AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            using var scope = ServiceScopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<TisaContext>(); 
            await context.Set<T>().AddRangeAsync(entities);
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> expression)
        {
            using var scope = ServiceScopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<TisaContext>(); 
            return await context.Set<T>().Where(expression).ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            using var scope = ServiceScopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<TisaContext>(); 
            return await context.Set<T>().ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            using var scope = ServiceScopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<TisaContext>(); 
            return await context.Set<T>().FindAsync(id);
        }

        public async Task RemoveAsync(T entity)
        {
            using var scope = ServiceScopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<TisaContext>(); 
            context.Set<T>().Remove(entity);
            await context.SaveChangesAsync();
        }

        public async Task RemoveRangeAsync(IEnumerable<T> entities)
        {
            using var scope = ServiceScopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<TisaContext>(); 
            context.Set<T>().RemoveRange(entities);
            await context.SaveChangesAsync();
        }
    }
}