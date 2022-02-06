using Microsoft.EntityFrameworkCore;
using ReportService.Core.Models;

namespace ReportService.Infrastructure.Context
{
    public class ReportServiceContext : DbContext
    {
        public DbSet<Report>? Reports { get; set; }

        public ReportServiceContext(DbContextOptions<ReportServiceContext> context) : base(context)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql("Host=localhost;Database=ReportServiceDB;Username=postgres;Password=123456");
    }
}
