using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Loymax.Domain.Data;
using Loymax.Domain.Data.ModelsDto;
using Loymax.Domain.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Loymax.Tests
{
    [TestClass]
    public class ClientControllerTests
    {
        private IClientsService ClientsService { get; set; }
        private ApplicationDbContext DbContext { get; set; }
        private static Random Random { get; } = new Random();
        private static int NumberOfClients { get; } = 50;

        [TestInitialize]
        public async Task Initialize()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "ClientsTestDb")
                .Options;
            DbContext = new ApplicationDbContext(options);
            var mapperMock = new Mock<IMapper>();
            ClientsService = new ClientsService(DbContext, mapperMock.Object);

            for (var i = 0; i < NumberOfClients; i++)
            {
                await ClientsService.RegisterClientAsync(new CreateClientDto
                {
                    FirstName = $"TestFirstName{i}",
                    LastName = $"TestLastName{i}",
                    DateOfBirth = new DateTime(Random.Next(1950, 1990), Random.Next(1, 13), Random.Next(1, 29)),
                    AccountBalance = Random.Next(1000, 2000)
                });
            }
            await DbContext.SaveChangesAsync();
        }

        [TestMethod]
        public void Register_AddClients_ShouldСontainAllClients()
        {
            Assert.AreEqual(DbContext.Clients.Count(), NumberOfClients);
        }
    }
}