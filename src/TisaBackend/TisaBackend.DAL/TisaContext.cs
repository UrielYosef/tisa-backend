using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TisaBackend.DAL.Auth;

namespace TisaBackend.DAL
{
    public class TisaContext : IdentityDbContext<User>
    {
        public TisaContext(DbContextOptions<TisaContext> options) : base(options)
        {

        }

        #region DBSets

        #endregion
    }
}