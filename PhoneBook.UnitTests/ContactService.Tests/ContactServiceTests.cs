using ContactService.API.Controllers;
using ContactService.Tests.Context;
using Microsoft.AspNetCore.Mvc;
using PersonService.Core.Models;
using System;
using System.Collections.Generic;
using Xunit;

namespace ContactService.Tests
{
    /// <summary>
    /// Contact Service API Unit Test Class
    /// </summary>
    public class ContactServiceTests : IClassFixture<ContactServiceFakeContext>
    {
        #region Members

        /// <summary>
        /// Person Controller object
        /// </summary>
        private readonly PersonController _personController;

        /// <summary>
        /// Fake DB Context for testing
        /// </summary>
        private readonly ContactServiceFakeContext _fakeContext;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        public ContactServiceTests(ContactServiceFakeContext context)
        {
            _fakeContext = context;

            _personController = new PersonController(_fakeContext.Context);
        }


        #endregion

        #region Methods

        [Fact]
        public void GetAllAsync_WhenCalled_ReturnsAllRegisteredData()
        {
            // Act
            var response = _personController.GetAllAsync();

            // Assert
            var items = Assert.IsType<List<Person>>(response.Result);
            Assert.Equal(3, items.Count);
        }

        [Theory]
        [InlineData("fc15759c-b40a-4e29-ba73-7176bd9f7d6b")]
        public void GetOneByIdAsync_WhenCalledWithRegesteredId_ReturnsOk(string id)
        {
            // Act
            var response = _personController.GetOneByIdAsync(id);
            var returnType = response.Result as OkObjectResult;
            var receivedObject = returnType.Value as Person;

            // Assert
            Assert.NotNull(response.Result);
            Assert.IsType<OkObjectResult>(response.Result);
            Assert.Equal(id, receivedObject.Id);
        }

        [Theory]
        [InlineData("random-id")]
        public void GetOneByIdAsync_WhenCalledWithUnregisteredId_ReturnsNotFound(string id)
        {
            // Act
            var response = _personController.GetOneByIdAsync(id);

            // Assert
            Assert.IsType<NotFoundResult>(response.Result);

        }

        [Theory]
        [InlineData(2)]
        public void GetOneInfoByIdAsync_WhenCalledWithRegisteredId_ReturnsOk(int id)
        {
            // Act
            var response = _personController.GetOneInfoByIdAsync(id);
            var returnType = response.Result as OkObjectResult;
            var receivedObject = returnType.Value as ContactInfo;

            // Assert
            Assert.NotNull(response.Result);
            Assert.IsType<OkObjectResult>(response.Result);
            Assert.Equal(id, receivedObject.Id);
        }

        [Theory]
        [InlineData(0)]
        public void GetOneInfoByIdAsync_WhenCalledWithUnRegisteredId_ReturnsNotFound(int id)
        {
            // Act
            var response = _personController.GetOneInfoByIdAsync(id);

            // Assert
            Assert.IsType<NotFoundResult>(response.Result);
        }

        [Fact]
        public void CreateAsync_WhenCalledWithNewPerson_ReturnsCreated()
        {
            // Arrange
            var person = new Person
            {
                Id = Guid.NewGuid().ToString(),
                Name = "TestName",
                Surname = "TestSurname",
                Company = "TestCompany",
                ContactInfo = new List<ContactInfo>
                {
                    new ContactInfo
                    {
                        Id = 5,
                        Location = "Eskişehir",
                        Email="test@test.com",
                        // Set an existing person id in fake context.
                        PersonId= "fc15759c-b40a-4e29-ba73-7176bd9f7d6b",
                        PhoneNumber = "0111 111 11 11"
                    }
                }
            };

            // Act
            var response = _personController.CreateAsync(person);

            // Assert
            Assert.IsType<CreatedResult>(response.Result);
        }

        [Fact]
        public void CreateAsync_WhenCalledWithInvalidModel_ReturnsBadRequest()
        {
            // Arrange
            // Send a person object that has empty parameters.
            var person = new Person();

            // Act
            var response = _personController.CreateAsync(person);

            // Assert
            Assert.IsType<BadRequestResult>(response.Result);
        }

        [Theory]
        [InlineData("fc15759c-b40a-4e29-ba73-7176bd9f7d6b")]
        public void UpdateAsync_WhenCalledRegisteredId_ReturnsNoContent(string id)
        {
            // Arrange
            var person = new Person
            {
                // Update company information
                Company = "NEW COMPANY",
                Name = "İsa",
                Surname = "Tutkun"
            };

            // Act 
            var response = _personController.UpdateAsync(id, person);

            // Assert
            Assert.IsType<NoContentResult>(response.Result);
        }

        [Theory]
        [InlineData("unregistered-id")]
        public void UpdateAsync_WhenCalledUnregisteredId_ReturnsNotFound(string id)
        {
            // Arrange
            var person = new Person
            {
                // Update company information
                Company = "NEW COMPANY",
                Name = "İsa",
                Surname = "Tutkun"
            };

            // Act 
            var response = _personController.UpdateAsync(id, person);

            // Assert
            Assert.IsType<NotFoundResult>(response.Result);
        }

        [Theory]
        [InlineData("1ff75fd5-d49c-458a-a923-3c9c5a884611")]
        public void DeleteAsync_WhenCalledRegisteredId_ReturnsNoContent(string id)
        {
            // Act 
            var response = _personController.DeleteAsync(id);

            // Assert
            Assert.IsType<NoContentResult>(response.Result);
        }

        [Theory]
        [InlineData("random-id")]
        public void DeleteAsync_WhenCalledUnregisteredId_ReturnsNoContent(string id)
        {
            // Act 
            var response = _personController.DeleteAsync(id);

            // Assert
            Assert.IsType<NotFoundResult>(response.Result);
        }

        [Fact]
        public void CreateInfoAsync_WhenCalledWithNewPerson_ReturnsCreated()
        {
            // Arrange
            var contactInfo = new ContactInfo
            {
                Id = 6,
                Location = "Eskişehir",
                Email = "test@test.com",
                // Set an existing person id in fake context.
                PersonId = "fc15759c-b40a-4e29-ba73-7176bd9f7d6b",
                PhoneNumber = "0111 111 11 11"
            };

            // Act
            var response = _personController.CreateInfoAsync(contactInfo);

            // Assert
            Assert.IsType<CreatedResult>(response.Result);
        }

        [Theory]
        [InlineData(3)]
        public void DeleteInfoAsync_WhenCalledRegisteredId_ReturnsNoContent(int id)
        {
            // Act 
            var response = _personController.DeleteInfoAsync(id);

            // Assert
            Assert.IsType<NoContentResult>(response.Result);
        }

        [Theory]
        [InlineData("Eskişehir")]
        public void GetNumberOfPersonByLocation_WhenCalled_ReturnsNumberOfPerson(string location)
        {
            // Act 
            var response = _personController.GetNumberOfPersonByLocation(location);

            // Assert
            Assert.IsType<int>(response.Result);
            Assert.Equal(1, response.Result);
        }

        [Theory]
        [InlineData("Eskişehir")]
        public void GetNumberOfPhoneNumbersByLocation_WhenCalled_ReturnsNumberOfPhoneNumbers(string location)
        {
            // Act 
            var response = _personController.GetNumberOfPersonByLocation(location);

            // Assert
            Assert.IsType<int>(response.Result);
            Assert.Equal(1, response.Result);
        }

        #endregion
    }
}