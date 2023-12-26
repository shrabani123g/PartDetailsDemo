using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using PartDetailsDemo.Controllers;
using PartDetailsDemo.CQRS.Commands;
using PartDetailsDemo.CQRS.Queries;
using PartDetailsDemo.Models;
using PartDetailsDemo.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TestProjectPartDemo.MockData;
using Xunit;

namespace TestProjectPartDemo.System.Controllers
{  
    public class TestPartController
    {
        private readonly Mock<IMediator> mediator;
        private readonly PartController partController;
        //private readonly ILogger<PartRepository> seriLogger;

        public TestPartController()
        {
            mediator = new Mock<IMediator>(MockBehavior.Strict);
            partController = new PartController(mediator.Object)
            {
                ControllerContext = { HttpContext = new DefaultHttpContext() }
            };
        }

        [Fact]
        public async Task GetAll_ReturnsAllResults()
        {
            ///Arrange
            //var partService = new Mock<IMediator>();
            var partData = PartMockData.GetParts();
            mediator.Setup(x => x.Send(It.IsAny<GetAllPartQuery>(), CancellationToken.None)).ReturnsAsync(partData).Verifiable();

            ///Act
            var response = await partController.GetAll();

            ///Assert
            Assert.IsType<OkObjectResult>(response);

            var value = ((ObjectResult)response).Value as List<Part>;

            Assert.Equal(3, value.Count);
            mediator.Verify();


        }

        [Fact]
        public async Task GetAll_ReturnsNoResults()
        {
            ///Arrange
            //var partService = new Mock<IMediator>();
            List<Part> partData = new List<Part>();
            mediator.Setup(x => x.Send(It.IsAny<GetAllPartQuery>(), CancellationToken.None)).ReturnsAsync(partData).Verifiable();

            ///Act
            var response = await partController.GetAll();

            ///Assert
            Assert.IsType<NoContentResult>(response);
            mediator.Verify();

        }

        [Fact]
        public async Task GetById_WithValidId_ShouldReturnPart()
        {
            ///Arrange
            var partId = 3;
            var partData = PartMockData.GetParts()[2];

            mediator.Setup(x => x.Send(It.IsAny<GetPartByIdQuery>(), CancellationToken.None)).ReturnsAsync(partData).Verifiable();

            ///Act
            var response = await partController.GetById(partId);
            ///Assert

            Assert.IsType<OkObjectResult>(response);

            var value = ((ObjectResult)response).Value as Part;

            Assert.Equal(partData, value);
            mediator.Verify();
        }        

        [Fact]
        public async Task GetById_WitInvalidId_ShouldNotFound()
        {
            ///Arrange
            var partId = 5;
            Part partData = null;

            mediator.Setup(x => x.Send(It.IsAny<GetPartByIdQuery>(), CancellationToken.None)).ReturnsAsync(partData).Verifiable();

            ///Act
            var response = await partController.GetById(partId);
            ///Assert

            Assert.IsType<NotFoundObjectResult>(response);

            var value = ((ObjectResult)response).Value as String;

            Assert.Equal("Part not found.", value);
            mediator.Verify();
        }

        [Fact]
        public async Task Create_WithValidData_ShouldReturnOk()
        {
            ///Arrange
            var affectedRow = 1;
            var partCommand = new CreatePartCommand();
            partCommand.PartName = "UnitTest";
            partCommand.PartDetails = "UnitTestDetails";

            mediator.Setup(x => x.Send(It.IsAny<CreatePartCommand>(), CancellationToken.None)).ReturnsAsync(affectedRow).Verifiable();

            ///Act
            var response = await partController.Create(partCommand);
            ///Assert

            Assert.IsType<OkObjectResult>(response);

            var value = ((ObjectResult)response).Value as String;

            Assert.Equal("Data is created successfully.", value);
            mediator.Verify();
        }

        [Fact]
        public async Task Create_WithInValidData_ShouldReturnBadRequest()
        {
            ///Arrange
            var affectedRow = 0;
            var partCommand = new CreatePartCommand();
            partCommand.PartName = "UnitTest";
            partCommand.PartDetails = "UnitTestDetails";

            mediator.Setup(x => x.Send(It.IsAny<CreatePartCommand>(), CancellationToken.None)).ReturnsAsync(affectedRow).Verifiable();

            ///Act
            var response = await partController.Create(partCommand);
            ///Assert

            Assert.IsType<BadRequestObjectResult>(response);

            var value = ((ObjectResult)response).Value as String;

            Assert.Equal("Data is not created.", value);
            mediator.Verify();
        }

        [Fact]
        public async Task Update_WithAValidId_ShouldReturnAffectedRow()
        {
            ///Arrange
            var affectedRow = 1;
            var partId = 3;
            var partCommand = new UpdatePartCommand();
            partCommand.PartName = "Test3";
            partCommand.PartDetails = "Test3DetailsUpdated";

            mediator.Setup(x => x.Send(It.IsAny<UpdatePartCommand>(), CancellationToken.None)).ReturnsAsync(affectedRow).Verifiable();

            ///Act
            var response = await partController.Update(partId, partCommand);

            ///Assert
            Assert.IsType<OkObjectResult>(response);

            var value = ((ObjectResult)response).Value as String;

            Assert.Equal("Data is updated successfully.", value);
            mediator.Verify();
        }

        [Fact]
        public async Task Update_WithNonExistentId_ShouldReturnNoRow()
        {
            ///Arrange
            var affectedRow = 0;
            var partId = 5;
            var partCommand = new UpdatePartCommand();
            partCommand.PartName = "Test3";
            partCommand.PartDetails = "Test3DetailsUpdated";

            mediator.Setup(x => x.Send(It.IsAny<UpdatePartCommand>(), CancellationToken.None)).ReturnsAsync(affectedRow).Verifiable();

            ///Act
            var response = await partController.Update(partId, partCommand);

            ///Assert
            Assert.IsType<OkObjectResult>(response);

            var value = ((ObjectResult)response).Value as String;

            Assert.Equal("Data not found.", value);
            mediator.Verify();
        }

        [Fact]
        public async Task Delete_WithAValidId_ShouldReturnAffectedRow()
        {
            ///Arrange
            var affectedRow = 1;
            var partId = 3;
         
            mediator.Setup(x => x.Send(It.IsAny<DeletePartByIdCommand>(), CancellationToken.None)).ReturnsAsync(affectedRow).Verifiable();

            ///Act
            var response = await partController.Delete(partId);

            ///Assert
            Assert.IsType<OkObjectResult>(response);

            var value = ((ObjectResult)response).Value as String;

            Assert.Equal("Data is deleted successfully.", value);
            mediator.Verify();
        }

        [Fact]
        public async Task Delete_WithAnInValidId_ShouldReturnNoRow()
        {
            ///Arrange
            var affectedRow = 0;
            var partId = 5;

            mediator.Setup(x => x.Send(It.IsAny<DeletePartByIdCommand>(), CancellationToken.None)).ReturnsAsync(affectedRow).Verifiable();

            ///Act
            var response = await partController.Delete(partId);

            ///Assert
            Assert.IsType<OkObjectResult>(response);

            var value = ((ObjectResult)response).Value as String;

            Assert.Equal("Data not found.", value);
            mediator.Verify();
        }

    }
}
