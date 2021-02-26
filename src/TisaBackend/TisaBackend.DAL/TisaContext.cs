using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace TisaBackend.DAL
{
    public class TisaContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public TisaContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        #region DBSets

        #endregion

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_configuration.GetConnectionString("DefaultConnection"));
        }
    }
}