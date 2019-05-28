using FuryTechs.WebApi.Example.Models.Database;
using Microsoft.EntityFrameworkCore;

namespace FuryTechs.WebApi.Example.Db
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext() : base()
        {
        }

        public DatabaseContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}
