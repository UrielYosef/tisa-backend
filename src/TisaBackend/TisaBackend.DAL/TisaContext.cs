using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using TisaBackend.Domain.Models;
using TisaBackend.Domain.Models.Auth;

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
        public virtual DbSet<DepartmentType> DepartmentTypes { get; set; }
        public virtual DbSet<AirplaneDepartmentSeats> AirplaneDepartmentSeats { get; set; }

        //Ongoing Data
        public virtual DbSet<UserToAirline> UserToAirline { get; set; }
        public virtual DbSet<Airline> Airlines { get; set; }
        public virtual DbSet<Airplane> Airplanes { get; set; }
        public virtual DbSet<Flight> Flights { get; set; }
        public virtual DbSet<DalDepartmentPrice> DepartmentPrices { get; set; }
        public virtual DbSet<FlightOrder> FlightOrders { get; set; }
        public virtual DbSet<DalReview> Reviews { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Airline>()
                .HasIndex(u => u.Name)
                .IsUnique();

            builder.Entity<UserToAirline>()
                .ToTable("UserToAirline");

            builder.Entity<DalDepartmentPrice>()
                .ToTable("DepartmentPrices");

            builder.Entity<DalReview>()
                .ToTable("Reviews");
        }
    }
}