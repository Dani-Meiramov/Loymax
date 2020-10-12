using System;
using Loymax.Domain.Data.ModelsDto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Loymax.Domain.Services
{
    public interface IClientsService
    {
        Task RegisterClientAsync(CreateClientDto client);
        Task<List<ClientDto>> GetAllClientsAsync();
        Task<ClientDto> GetClientByIdAsync(Guid id);
        Task EditClientAsync(EditClientDto client);
        Task DeleteClientByIdAsync(Guid id);
    }
}