using Microsoft.EntityFrameworkCore;
using ReportService.Core.Models;

namespace ReportService.API.Context
{
    public class ReportServiceContext : DbContext
    {
        public DbSet<Report>? Reports { get; set; }

        public ReportServiceContext(DbContextOptions<ReportServiceContext> context) : base(context)
        {
            // Added for timestamp logging
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }
    }
}
