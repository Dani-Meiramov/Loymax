using System;
using System.Threading.Tasks;
using Loymax.Domain.Data.ModelsDto;

namespace Loymax.Domain.Services
{
    public interface IFinanceService
    {
        Task<decimal> GetBalanceByIdAsync(Guid id);
        Task<decimal> EnrollAsync(ClientOperationDto clientOperation);
        Task<decimal> WithdrawAsync(ClientOperationDto clientOperation);
    }
}