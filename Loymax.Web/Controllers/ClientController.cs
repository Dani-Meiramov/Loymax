using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Loymax.Domain.Data.ModelsDto;
using Loymax.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace Loymax.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private IClientsService ClientService { get; }

        public ClientController(IClientsService clientService)
        {
            ClientService = clientService;
        }

        [HttpGet]
        public async Task<ICollection<ClientDto>> Get()
        {
            return await ClientService.GetAllClientsAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ClientDto>> Get(Guid id)
        {
            try
            {
                return await ClientService.GetClientByIdAsync(id);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateClientDto client)
        {
            if (client == null || !ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                await ClientService.RegisterClientAsync(client);
                return Ok();
            }
            catch
            {
                return StatusCode(500, "Error appears during registration");
            }
        }

        [HttpPut]
        public async Task<ActionResult> Put(EditClientDto client)
        {
            if (client == null || !ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                await ClientService.EditClientAsync(client);
                return Ok();
            }
            catch
            {
                return StatusCode(500, "Error appears during editing");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                await ClientService.DeleteClientByIdAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}