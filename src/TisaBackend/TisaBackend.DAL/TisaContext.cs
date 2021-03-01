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
        public virtual DbSet<AirplaneDepartmentSeats> AirplaneDepartmentSeats { get; set; }

        //Ongoing Data
        public virtual DbSet<Airline> Airlines { get; set; }
        public virtual DbSet<Airplane> Airplanes { get; set; }
        public virtual DbSet<Flight> Flights { get; set; }
        public virtual DbSet<FlightPrice> FlightPrices { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<FlightPrice>().HasKey(flightPrice => new {
                flightPrice.FlightId,
                flightPrice.DepartmentType
            });
        }
    }
}