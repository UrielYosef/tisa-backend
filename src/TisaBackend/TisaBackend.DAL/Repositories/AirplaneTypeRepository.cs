using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TisaBackend.Domain.Interfaces.DAL;
using TisaBackend.Domain.Models;

namespace TisaBackend.DAL.Repositories
{
    public class AirplaneTypeRepository : Repository<AirplaneType>, IAirplaneTypeRepository
    {
        public AirplaneTypeRepository(IServiceScopeFactory serviceScopeFactory) : base(serviceScopeFactory)
        {

        }

        public async Task<IList<DepartmentType>> GetDepartmentTypesAsync(int airplaneTypeId)
        {
            using var scope = ServiceScopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<TisaContext>();

            return await context.AirplaneDepartmentSeats
                .Include(airplaneDepartmentSeats => airplaneDepartmentSeats.DepartmentType)
                .Where(airplaneDepartmentSeats => airplaneDepartmentSeats.AirplaneTypeId.Equals(airplaneTypeId))
                .Select(airplaneDepartmentSeats => airplaneDepartmentSeats.DepartmentType)
                .ToListAsync();
        }

        public async Task AddDepartmentTypeAsync(DepartmentType departmentType)
        {
            using var scope = ServiceScopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<TisaContext>();

            await context.DepartmentTypes.AddAsync(departmentType);
            await context.SaveChangesAsync();
        }

        public async Task AddSeatsToAirplaneTypeDepartmentAsync(AirplaneDepartmentSeats airplaneDepartmentSeats)
        {
            using var scope = ServiceScopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<TisaContext>();

            await context.AirplaneDepartmentSeats.AddAsync(airplaneDepartmentSeats);
            await context.SaveChangesAsync();
        }
    }
}