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
    public class FinanceController : ControllerBase
    {
        private IFinanceService FinanceService { get; }

        public FinanceController(IFinanceService financeService)
        {
            FinanceService = financeService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<decimal>> Get(Guid id)
        {
            try
            {
                return await FinanceService.GetBalanceByIdAsync(id);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(ClientOperationDto clientOperation)
        {
            try
            {
                decimal currentBalance;
                switch (clientOperation.Type)
                {
                    case OperationType.Enroll:
                        currentBalance = await FinanceService.EnrollAsync(clientOperation);
                        return Ok($"Money was succesfully enrolled. Current Balance = {currentBalance}");
                    case OperationType.Withdraw:
                        currentBalance = await FinanceService.WithdrawAsync(clientOperation);
                        return Ok($"Money was succesfully withdrawed. Current Balance = {currentBalance}");
                    default:
                        return BadRequest("Operation type not recognized");
                }
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}