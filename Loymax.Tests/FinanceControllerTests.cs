using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Loymax.Domain.Data;
using Loymax.Domain.Data.ModelsDto;
using Loymax.Domain.Data.Models;
using Loymax.Domain.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Loymax.Tests
{
    [TestClass]
    public class FinanceControllerTests
    {
        private IClientsService ClientsService { get; set; }
        private IFinanceService FinanceService { get; set; }
        private ApplicationDbContext DbContext { get; set; }
        private static Random Random { get; } = new Random();
        private static bool IsErrors { get; set; }
        private static int NumberOfClients { get; } = 50;
        private static object Locker { get; } = new object();

        /// <summary>
        /// Preparing for the test: registration of 50 users
        /// </summary>
        /// <returns></returns>
        [TestInitialize]
        public async Task Initialize()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "ClientsTestDb")
                .Options;
            DbContext = new ApplicationDbContext(options);
            var mapperMock = new Mock<IMapper>();
            ClientsService = new ClientsService(DbContext, mapperMock.Object);
            FinanceService = new FinanceService(DbContext);

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

        /// <summary>
        /// Performing enrollments/withdrawals to 50 users in 10 threads
        /// </summary>
        [TestMethod]
        public void EnrollAndWithdraw_WithValidAmount_UpdatesBalance()
        {
            const int threadsCount = 10;
            var clientCount = DbContext.Clients.Count();
            var clientsPerThread = clientCount / threadsCount;
            var listThreads = new List<Thread>();

            for (var i = 0; i < threadsCount; i++)
            {
                var clients = DbContext.Clients.Skip(i * clientsPerThread)
                    .Take(clientsPerThread).ToList();

                var myThread = new Thread(TestClients);
                listThreads.Add(myThread);
                myThread.Start(clients);
            }

            while (listThreads.Any(x => x.IsAlive))
            {
            }

            Assert.AreEqual(IsErrors, false);
        }

        private void TestClients(object listClients)
        {
            if (!(listClients is List<Client> castedListClients)) return;
            foreach (var client in castedListClients)
            {
                var startBalance = client.AccountBalance;
                // Enrollment amount knowingly more than withdrawal amount to avoid exceptions
                var enrollmentAmount = Random.Next(1000, 2000);
                var withdrawalAmount = Random.Next(500, 900);
                var expectedBalance = startBalance + enrollmentAmount - withdrawalAmount;

                lock (Locker)
                {
                    Task.Run(() => FinanceService.EnrollAsync(new ClientOperationDto()
                        {Id = client.Id, TransactionAmount = enrollmentAmount})).Wait();
                    Task.Run(() => FinanceService.WithdrawAsync(new ClientOperationDto()
                        {Id = client.Id, TransactionAmount = withdrawalAmount})).Wait();
                    var currentBalance = Task.Run(() => FinanceService.GetBalanceByIdAsync(client.Id)).Result;

                    if (expectedBalance != currentBalance)
                    {
                        IsErrors = true;
                        break;
                    }
                }
            }
        }
    }
}