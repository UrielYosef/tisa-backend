using System.Threading.Tasks;
using TisaBackend.Domain.Interfaces;

namespace TisaBackend.DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TisaContext _context;

        public UnitOfWork(TisaContext context)
        {
            _context = context;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async void Dispose()
        {
            await _context.DisposeAsync();
        }
    }
}