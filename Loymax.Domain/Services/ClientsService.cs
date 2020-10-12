using AutoMapper;
using Loymax.Domain.Data.ModelsDto;
using Loymax.Domain.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Loymax.Domain.Data;

namespace Loymax.Domain.Services
{
    public class ClientsService : IClientsService
    {
        private ApplicationDbContext DbContext { get; }
        private IMapper Mapper { get; }

        public ClientsService(ApplicationDbContext dbContext, IMapper mapper)
        {
            DbContext = dbContext;
            Mapper = mapper;
        }

        public async Task RegisterClientAsync(CreateClientDto client)
        {
            if (await DbContext.Clients.FirstOrDefaultAsync(c =>
                c.FirstName == client.FirstName
                && c.LastName == client.LastName
                && c.Patronymic == client.Patronymic
                && c.DateOfBirth == client.DateOfBirth) == null)
            {
                var mapped = new Client()
                {
                    LastName = client.LastName,
                    FirstName = client.FirstName,
                    Patronymic = client.Patronymic,
                    DateOfBirth = client.DateOfBirth,
                    AccountBalance = client.AccountBalance
                };
                await DbContext.Clients.AddAsync(mapped);
                await DbContext.SaveChangesAsync();
            }
            else
            {
                throw new ArgumentException();
            }
        }

        public async Task<List<ClientDto>> GetAllClientsAsync()
        {
            var allClients = await DbContext.Clients
                .Where(c => c.IsDeleted == false).ToListAsync();
            return Mapper.Map<List<ClientDto>>(allClients);
        }

        public async Task<ClientDto> GetClientByIdAsync(Guid id)
        {
            var client = await GetClientAsync(id);
            return Mapper.Map<ClientDto>(client);
        }

        public async Task EditClientAsync(EditClientDto editClientDto)
        {
            var client = await GetClientAsync(editClientDto.Id);
            client.LastName = editClientDto.LastName;
            client.FirstName = editClientDto.FirstName;
            client.DateOfBirth = editClientDto.DateOfBirth;
            client.Patronymic = editClientDto.Patronymic;
            client.AccountBalance = editClientDto.AccountBalance;
            DbContext.Clients.Update(client);
            await DbContext.SaveChangesAsync();
        }

        public async Task DeleteClientByIdAsync(Guid id)
        {
            var client = await GetClientAsync(id);
            DbContext.Clients.Remove(client);
            await DbContext.SaveChangesAsync();
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