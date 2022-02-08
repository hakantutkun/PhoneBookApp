using Microsoft.AspNetCore.Mvc;
using ReportService.API;
using ReportService.API.Controllers;
using ReportService.Core.Models;
using ReportService.Core.Mqtt;
using ReportService.Tests.Context;
using System;
using System.Collections.Generic;
using Xunit;

namespace ReportService.Tests
{
    public class ReportServiceTests : IClassFixture<ReportServiceFakeContext>
    {
        #region Members

        /// <summary>
        /// Report Controller object
        /// </summary>
        private readonly ReportController _reportController;

        /// <summary>
        /// Fake DB Context for testing
        /// </summary>
        private readonly ReportServiceFakeContext _fakeContext;

        private readonly MqttServer _mqttServer;

        private readonly WorkerService _workerService;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        public ReportServiceTests(ReportServiceFakeContext context)
        {
            _fakeContext = context;

            _mqttServer = new MqttServer();

            _reportController = new ReportController(_fakeContext.Context,_mqttServer,_workerService);
        }

        #endregion

        [Fact]
        public void GetAllAsync_WhenCalled_ReturnsAllRegisteredData()
        {
            // Act
            var response = _reportController.GetAllAsync();

            // Assert
            var items = Assert.IsType<List<Report>>(response.Result);
            Assert.Equal(3, items.Count);
        }

        [Theory]
        [InlineData("fc15759c-b40a-4e29-ba73-7176bd9f7d6b")]
        public void GetOneByIdAsync_WhenCalledWithRegesteredId_ReturnsOk(string id)
        {
            // Act
            var response = _reportController.GetOneByIdAsync(id);
            var returnType = response.Result as OkObjectResult;
            var receivedObject = returnType.Value as Report;

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
            var response = _reportController.GetOneByIdAsync(id);

            // Assert
            Assert.IsType<NotFoundResult>(response.Result);

        }

        [Fact]
        public void CreateAsync_WhenCalledWithNewReport_ReturnsCreated()
        {
            // Arrange
            var report = new Report
            {
                Id = Guid.NewGuid().ToString(),
                CreationTime = DateTime.Now,
                Location = "Eskişehir",
                ReportState = Core.Enums.ReportState.Preparing
            };

            // Act
            var response = _reportController.CreateAsync(report);

            // Assert
            Assert.IsType<CreatedResult>(response.Result);
        }

        [Fact]
        public void CreateAsync_WhenCalledWithInvalidModel_ReturnsBadRequest()
        {
            // Arrange
            // Send a report object that has empty parameters.
            var report = new Report();

            // Act
            var response = _reportController.CreateAsync(report);

            // Assert
            Assert.IsType<BadRequestResult>(response.Result);
        }

        [Theory]
        [InlineData("1ff75fd5-d49c-458a-a923-3c9c5a884611")]
        public void DeleteAsync_WhenCalledRegisteredId_ReturnsNoContent(string id)
        {
            // Act 
            var response = _reportController.DeleteAsync(id);

            // Assert
            Assert.IsType<NoContentResult>(response.Result);
        }

        [Theory]
        [InlineData("random-id")]
        public void DeleteAsync_WhenCalledUnregisteredId_ReturnsNoContent(string id)
        {
            // Act 
            var response = _reportController.DeleteAsync(id);

            // Assert
            Assert.IsType<NotFoundResult>(response.Result);
        }

    }
}