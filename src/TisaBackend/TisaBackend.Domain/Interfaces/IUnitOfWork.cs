using System;
using System.Threading.Tasks;

namespace TisaBackend.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> SaveChangesAsync();
    }
}