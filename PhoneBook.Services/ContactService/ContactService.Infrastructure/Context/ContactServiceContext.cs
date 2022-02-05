using Microsoft.EntityFrameworkCore;
using PersonService.Core.Models;

namespace ContactService.Infrastructure.Context
{
    public class ContactServiceContext : DbContext
    {
        public DbSet<Person>? Persons { get; set; }
        public DbSet<ContactInfo>? ContactInfos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql("Host=localhost;Database=ContactServiceDB;Username=postgres;Password=123456");
    }
}
