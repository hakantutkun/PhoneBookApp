using ContactService.API.Context;
using Microsoft.EntityFrameworkCore;
using PersonService.Core.Models;
using System;
using System.Collections.Generic;

namespace ContactService.Tests.Context
{
    public class ContactServiceFakeContext : IDisposable
    {
        public ContactServiceContext Context { get; private set; }

        public ContactServiceFakeContext() : base()
        {
            var options = new DbContextOptionsBuilder<ContactServiceContext>()
            .UseInMemoryDatabase("ContactServiceDatabase")
            .EnableSensitiveDataLogging(true)
            .Options;

            Context = new ContactServiceContext(options);

            Context.Persons.Add(new Person 
            { 
                Id = "c56f3f98-4566-404e-91c8-00e2a22298d5", 
                Company = "Company 1", 
                Name = "Hakan", 
                Surname = "Tutkun", 
                ContactInfo = new List<ContactInfo>() 
                { 
                    new ContactInfo 
                    { 
                        Id=1, Location = "Eskişehir", Email = "hakan.tutkun@gmail.com", PhoneNumber = "0533 333 33 33" 
                    },
                    new ContactInfo
                    {
                        Id=2, Location = "Ankara", Email = "hakan.tutkun@hotmail.com", PhoneNumber = "0222 222 22 22"
                    }
                } 
            });

            Context.Persons.Add(new Person
            {
                Id = "fc15759c-b40a-4e29-ba73-7176bd9f7d6b",
                Company = "Company 2",
                Name = "İsa",
                Surname = "Tutkun",
                ContactInfo = new List<ContactInfo>()
                {
                    new ContactInfo
                    {
                        Id=3, Location = "Istanbul", Email = "isa.tutkun@gmail.com", PhoneNumber = "0555 555 55 55"
                    },
                    new ContactInfo
                    {
                        Id=4,Location = "İzmir", Email = "isa.tutkun@hotmail.com", PhoneNumber = "0312 312 12 12"
                    }
                }
            });

            Context.Persons.Add(new Person
            {
                Id = "1ff75fd5-d49c-458a-a923-3c9c5a884611",
                Company = "Company 2",
                Name = "İsa",
                Surname = "Tutkun"
            });

            Context.SaveChanges();
        }

        public void Dispose()
        {
            Context.Dispose();
        }
    }
}
