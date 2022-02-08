using Microsoft.EntityFrameworkCore;
using ReportService.API.Context;
using ReportService.Core.Models;
using System;

namespace ReportService.Tests.Context
{
    public class ReportServiceFakeContext : IDisposable
    {
        public ReportServiceContext Context { get; private set; }

        public ReportServiceFakeContext() : base()
        {
            var options = new DbContextOptionsBuilder<ReportServiceContext>()
            .UseInMemoryDatabase("ReportServiceDatabase")
            .EnableSensitiveDataLogging(true)
            .Options;

            Context = new ReportServiceContext(options);

            Context.Reports.Add(new Report
            {
                Id = "c56f3f98-4566-404e-91c8-00e2a22298d5",
                CreationTime = DateTime.Now,
                Location = "Eskişehir"
            });

            Context.Reports.Add(new Report
            {
                Id = "fc15759c-b40a-4e29-ba73-7176bd9f7d6b",
                CreationTime = DateTime.Now,
                Location = "Eskişehir"
            });

            Context.Reports.Add(new Report
            {
                Id = "1ff75fd5-d49c-458a-a923-3c9c5a884611",
                CreationTime = DateTime.Now,
                Location = "Eskişehir"
            });

            Context.SaveChanges();
        }

        public void Dispose()
        {
            Context.Dispose();
        }
    }
}
