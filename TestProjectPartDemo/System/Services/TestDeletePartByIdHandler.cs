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
    public class TestDeletePartByIdHandler
    {
        private readonly Mock<IPartRepository> partRepository;
        private readonly DeletePartByIdCommandHandler deletePartByIdCommandHandler;

        public TestDeletePartByIdHandler()
        {
            partRepository = new Mock<IPartRepository>(MockBehavior.Strict);
            deletePartByIdCommandHandler = new DeletePartByIdCommandHandler(partRepository.Object);
        }

        [Fact]
        public async Task DeleteHandle_WithValidData_ShouldReturnAffectedRow()
        {
            ///Arrange   
            var affectedRow = 1;
            var deletePartByIdCommand = new DeletePartByIdCommand { PartId = 3 };
           
            partRepository.Setup(x => x.DeletePart(deletePartByIdCommand)).ReturnsAsync(affectedRow).Verifiable();

            ///Act
            var response = await deletePartByIdCommandHandler.Handle(deletePartByIdCommand, CancellationToken.None);

            ///Assert
            Assert.True(response == 1);

            partRepository.Verify(x => x.DeletePart(deletePartByIdCommand));

        }

        [Fact]
        public async Task DeleteHandle_WithNonExistentId_ShouldReturnNoRow()
        {
            ///Arrange   
            var affectedRow = 0;
            var deletePartByIdCommand = new DeletePartByIdCommand { PartId = 3234 };

            partRepository.Setup(x => x.DeletePart(deletePartByIdCommand)).ReturnsAsync(affectedRow).Verifiable();

            ///Act
            var response = await deletePartByIdCommandHandler.Handle(deletePartByIdCommand, CancellationToken.None);

            ///Assert
            Assert.True(response == 0);

            partRepository.Verify(x => x.DeletePart(deletePartByIdCommand));

        }
    }
}
