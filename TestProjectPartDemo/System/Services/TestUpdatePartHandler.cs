using Moq;
using PartDetailsDemo.CQRS.Commands;
using PartDetailsDemo.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TestProjectPartDemo.MockData;
using Xunit;

namespace TestProjectPartDemo.System.Services
{
    public class TestUpdatePartHandler
    {
        private readonly Mock<IPartRepository> partRepository;
        private readonly UpdatePartCommandHandler updatePartCommandHandler;

        public TestUpdatePartHandler()
        {
            partRepository = new Mock<IPartRepository>(MockBehavior.Strict);
            updatePartCommandHandler = new UpdatePartCommandHandler(partRepository.Object);
        }

        [Fact]
        public async Task UpdateHandle_WithValidData_ShouldReturnAffectedRow()
        {
            ///Arrange     
            var affectedRow = 1;
            var partCommand = new UpdatePartCommand();
            partCommand.PartId = 3;
            partCommand.PartName = "Test3";
            partCommand.PartDetails = "Test3DetailsUpdated";

            partRepository.Setup(x => x.UpdatePart(partCommand)).ReturnsAsync(affectedRow).Verifiable();

            //mediator.Setup(x => x.Send(It.IsAny<GetPartByIdQuery>(), CancellationToken.None)).ReturnsAsync(partData).Verifiable();

            ///Act
            var response = await updatePartCommandHandler.Handle(partCommand, CancellationToken.None);

            ///Assert
            Assert.True(response == 1);

            partRepository.Verify(x => x.UpdatePart(partCommand));

        }

        [Fact]
        public async Task UpdateHandle_WithNonExistentId_ShouldReturnNoRow()
        {
            ///Arrange     
            var affectedRow = 0;
            var partCommand = new UpdatePartCommand();
            partCommand.PartId = 36578;
            partCommand.PartName = "Test3";
            partCommand.PartDetails = "Test3DetailsUpdated";

            partRepository.Setup(x => x.UpdatePart(partCommand)).ReturnsAsync(affectedRow).Verifiable();

            //mediator.Setup(x => x.Send(It.IsAny<GetPartByIdQuery>(), CancellationToken.None)).ReturnsAsync(partData).Verifiable();

            ///Act
            var response = await updatePartCommandHandler.Handle(partCommand, CancellationToken.None);

            ///Assert
            Assert.True(response == 0);

            partRepository.Verify(x => x.UpdatePart(partCommand));

        }
    }
}
