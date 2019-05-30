using System;
using System.Collections.Generic;
using FuryTechs.WebApi.Example.Models.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FuryTechs.WebApi.Example.Db
{
    public class DatabaseContext : DbContext
    {
        private readonly ILoggerFactory _loggerFactory;

        public DatabaseContext() : base()
        {
            Database.EnsureCreated();
        }

        public DatabaseContext(DbContextOptions options, ILoggerFactory loggerFactory) : base(options)
        {
            _loggerFactory = loggerFactory;
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseLoggerFactory(_loggerFactory);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(x => x.SentMessages)
                .WithOne(x => x.Sender)
                .HasForeignKey(x => x.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Message>()
                .HasMany(x => x.Recipients)
                .WithOne(x => x.Message)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MessageRecipient>()
                .HasOne(x => x.To);
            Seed(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }

        private void Seed(ModelBuilder mb)
        {
            var userNo1 = new User
            {
                Id = Guid.Parse("778BB067-BA28-48FC-9A34-E9587F1939BA"),
                FirstName = "Balazs",
                LastName = "Gallay",
                Email = "gallayb@gmail.com",
            };

            mb.Entity<User>().HasData(userNo1);

            mb.Entity<Message>().HasData(new Message
            {
                Id = Guid.Parse("19FA2DB1-A4F6-4D36-A4FB-B94D7670CA0D"),
                CreatedAt = DateTimeOffset.Now,
                SenderId = userNo1.Id,
                Subject = "Test",
                Text = "This is a test message"
            });
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Message> Messages { get; set; }

        public DbSet<MessageRecipient> MessageRecipients { get; set; }
    }
}
