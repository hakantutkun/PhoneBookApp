using Microsoft.EntityFrameworkCore;
using PersonService.Core.Models;

namespace ContactService.API.Context
{
    public class ContactServiceContext : DbContext
    {
        public DbSet<Person>? Persons { get; set; }
        public DbSet<ContactInfo>? ContactInfos { get; set; }

        public ContactServiceContext(DbContextOptions<ContactServiceContext> context) : base(context)
        {

        }
    }
}
