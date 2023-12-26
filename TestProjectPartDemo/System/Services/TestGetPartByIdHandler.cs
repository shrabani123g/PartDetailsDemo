using MediatR;
using Moq;
using PartDetailsDemo.CQRS.Queries;
using PartDetailsDemo.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using PartDetailsDemo.Models;
using System.Threading;
using TestProjectPartDemo.MockData;

namespace TestProjectPartDemo.System.Services
{
    public class TestGetPartByIdHandler
    {
        private readonly Mock<IPartRepository> partRepository;
        private readonly GetPartByIdQueryHandler getPartByIdQueryHandler;

        public TestGetPartByIdHandler()
        {
            partRepository = new Mock<IPartRepository>(MockBehavior.Strict);
            getPartByIdQueryHandler = new GetPartByIdQueryHandler(partRepository.Object);
        }

        [Fact]
        public async Task GetByIdHandle_ShouldReturnValue_WithValidId()
        {
            ///Arrange     

            var partData = PartMockData.GetParts()[2];
            var getPartByIdQuery = new GetPartByIdQuery { Id = 1010 };

            partRepository.Setup(x => x.GetPartById(getPartByIdQuery)).ReturnsAsync(partData).Verifiable();

            //mediator.Setup(x => x.Send(It.IsAny<GetPartByIdQuery>(), CancellationToken.None)).ReturnsAsync(partData).Verifiable();

            ///Act
            var response = await getPartByIdQueryHandler.Handle(getPartByIdQuery, CancellationToken.None);

            ///Assert
            Assert.NotNull(response);

            partRepository.Verify(x => x.GetPartById(getPartByIdQuery));     

        }

        [Fact]
        public async Task GetByIdHandle_ShouldReturnNull_WithInvalidId()
        {
            ///Arrange     

            Part partData = null;
            var getPartByIdQuery = new GetPartByIdQuery { Id = 1234 };

            partRepository.Setup(x => x.GetPartById(getPartByIdQuery)).ReturnsAsync(partData).Verifiable();

            //mediator.Setup(x => x.Send(It.IsAny<GetPartByIdQuery>(), CancellationToken.None)).ReturnsAsync(partData).Verifiable();

            ///Act
            var response = await getPartByIdQueryHandler.Handle(getPartByIdQuery, CancellationToken.None);

            ///Assert
            Assert.Null(response);

            partRepository.Verify(x => x.GetPartById(getPartByIdQuery));

        }

    }
}
