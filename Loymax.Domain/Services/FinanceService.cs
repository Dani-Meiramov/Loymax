using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Loymax.Domain.Data;
using Loymax.Domain.Data.Models;
using Loymax.Domain.Data.ModelsDto;
using Microsoft.EntityFrameworkCore;

namespace Loymax.Domain.Services
{
    public class FinanceService : IFinanceService
    {
        private ApplicationDbContext DbContext { get; }

        public FinanceService(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task<decimal> GetBalanceByIdAsync(Guid id)
        {
            var client = await GetClientAsync(id);
            return client.AccountBalance;
        }

        public async Task<decimal> EnrollAsync(ClientOperationDto clientOperation)
        {
            var client = await GetClientAsync(clientOperation.Id);
            if (clientOperation.TransactionAmount <= 0)
            {
                throw new ArgumentException("The enrollment amount must be greater than zero");
            }
            client.AccountBalance += clientOperation.TransactionAmount;
            await DbContext.SaveChangesAsync();
            return client.AccountBalance;
        }

        public async Task<decimal> WithdrawAsync(ClientOperationDto clientOperation)
        {
            var client = await GetClientAsync(clientOperation.Id);
            if (clientOperation.TransactionAmount <= 0)
            {
                throw new ArgumentException("The withdrawal amount must be greater than zero");
            }
            if (clientOperation.TransactionAmount > client.AccountBalance)
            {
                throw new ArgumentException("Not enough money in the account");
            }
            client.AccountBalance -= clientOperation.TransactionAmount;
            await DbContext.SaveChangesAsync();
            return client.AccountBalance;
        }

        private async Task<Client> GetClientAsync(Guid id)
        {
            var client = await DbContext.Clients.FirstOrDefaultAsync(c => c.Id == id
                && c.IsDeleted == false);
            if (client == null)
            {
                throw new KeyNotFoundException($"Client with id = {id} does not found");
            }
            return client;
        }
    }
}