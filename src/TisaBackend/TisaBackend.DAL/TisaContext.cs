using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TisaBackend.DAL.Auth;
using TisaBackend.Domain.Models;

namespace TisaBackend.DAL
{
    public class TisaContext : IdentityDbContext<User>
    {
        public TisaContext(DbContextOptions<TisaContext> options) : base(options)
        {

        }

        #region DBSets

        //Static Data
        public virtual DbSet<Airport> Airports { get; set; }
        public virtual DbSet<AirplaneType> AirplaneTypes { get; set; }
        public virtual DbSet<AirplaneDepartmentType> AirplaneDepartmentTypes { get; set; }
        public virtual DbSet<AirplaneDepartmentSeats> AirplaneDepartmentSeats { get; set; }

        //Ongoing Data
        public virtual DbSet<Airline> Airlines { get; set; }
        public virtual DbSet<Airplane> Airplanes { get; set; }

        #endregion
    }
}