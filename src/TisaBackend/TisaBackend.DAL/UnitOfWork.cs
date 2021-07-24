using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using TisaBackend.Domain.Interfaces;
using TisaBackend.Domain.Interfaces.DAL;
using TisaBackend.Domain.Models;

namespace TisaBackend.DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public UnitOfWork(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task<int> SaveChangesAsync()
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<TisaContext>(); 
            return await context.SaveChangesAsync();
        }

        public async void Dispose()
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<TisaContext>(); 
            await context.DisposeAsync();
        }
    }
}