using Moq;
using PartDetailsDemo.CQRS.Commands;
using PartDetailsDemo.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace TestProjectPartDemo.System.Services
{
    public class TestCreatePartHandler
    {
        private readonly Mock<IPartRepository> partRepository;
        private readonly CreatePartCommandHandler createPartCommandHandler;

        public TestCreatePartHandler()
        {
            partRepository = new Mock<IPartRepository>(MockBehavior.Strict);
            createPartCommandHandler = new CreatePartCommandHandler(partRepository.Object);
        }

        [Fact]
        public async Task CreateHandle_WithValidData_ShouldReturnAffectedRow()
        {
            ///Arrange
            var partCommand = new CreatePartCommand();
            partCommand.PartName = "Part1";
            partCommand.PartDetails = "Part Details";
            var rowAffected = 1;

            partRepository.Setup(x => x.CreatePart(partCommand)).ReturnsAsync(rowAffected).Verifiable();

            ///Act
            var response = await createPartCommandHandler.Handle(partCommand, CancellationToken.None);

            ///Assert
            Assert.True(response == 1);

            partRepository.Verify(x => x.CreatePart(partCommand));
        }

        [Fact]
        public async Task CreateHandle_WithInValidData_ShouldReturnNoRow()
        {
            ///Arrange
            var partCommand = new CreatePartCommand();
            partCommand.PartName = "UnitTest";
            partCommand.PartDetails = "UnitTestDetails";
            var rowAffected = 0;

            partRepository.Setup(x => x.CreatePart(partCommand)).ReturnsAsync(rowAffected).Verifiable();

            ///Act
            var response = await createPartCommandHandler.Handle(partCommand, CancellationToken.None);

            ///Assert
            Assert.True(response == 0);

            partRepository.Verify(x => x.CreatePart(partCommand));
        }
    }
}
