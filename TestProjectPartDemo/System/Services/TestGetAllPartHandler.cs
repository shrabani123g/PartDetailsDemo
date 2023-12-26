using Microsoft.AspNetCore.Mvc;
using Moq;
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

namespace TestProjectPartDemo.System.Services
{
    public class TestGetAllPartHandler
    {
        private readonly Mock<IPartRepository> partRepository;
        private readonly GetAllPartQuery getAllPartQuery;
        private readonly GetAllPartQueryHandler getAllPartQueryHandler;

        public TestGetAllPartHandler()
        {
            partRepository = new Mock<IPartRepository>(MockBehavior.Strict);
            getAllPartQueryHandler = new GetAllPartQueryHandler(partRepository.Object);
            getAllPartQuery = new GetAllPartQuery();
        }

        [Fact]
        public async Task GetAllHandle_ShouldReturnValue()
        {
            ///Arrange    
            var partData = PartMockData.GetParts();
            partRepository.Setup(x => x.GetAllParts()).ReturnsAsync(partData).Verifiable();

            ///Act
            var response = await getAllPartQueryHandler.Handle(getAllPartQuery, CancellationToken.None);

            ///Assert
            Assert.NotNull(response);

            var value = response as List<Part>;
            Assert.True(value.Count > 0);

            partRepository.Verify(x => x.GetAllParts());

        }

        [Fact]
        public async Task GetAllHandle_ShouldNotReturnValue()
        {
            ///Arrange    
            //var partData = PartMockData.GetParts();
            var partData = new List<Part>();
            partRepository.Setup(x => x.GetAllParts()).ReturnsAsync(partData).Verifiable();

            ///Act
            var response = await getAllPartQueryHandler.Handle(getAllPartQuery, CancellationToken.None);

            ///Assert
            Assert.NotNull(response);

            //var value = ((ObjectResult)response).Value as List<Part>;
            var value = response as List<Part>;

            Assert.True(value.Count == 0);

            partRepository.Verify(x => x.GetAllParts());

        }
    }
}
