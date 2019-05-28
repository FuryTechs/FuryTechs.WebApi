using System;
using FuryTechs.WebApi.Base.Db.Model;
using Microsoft.EntityFrameworkCore;

namespace FuryTechs.WebApi.Base.Db
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext() : base()
        {
        }

        public DatabaseContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().HasData(new[] {
                new User { Id = 1, FirstName = "Melinda", LastName = "Gallayné Vrhovina", Email = "vrhovina.melinda@gmail.com" },
                new User { Id = 2, FirstName = "Balázs", LastName = "Gallay", Email="gallayb@gmail.com"},
            });
        }

        public DbSet<User> Users { get; set; }
    }
}
