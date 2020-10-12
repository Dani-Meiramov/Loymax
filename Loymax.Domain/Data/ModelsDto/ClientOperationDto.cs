using System;

namespace Loymax.Domain.Data.ModelsDto
{
    public class ClientOperationDto
    {
        public Guid Id { get; set; }
        public decimal TransactionAmount { get; set; }
        public OperationType Type { get; set; }
    }

    public enum OperationType
    {
        Enroll,
        Withdraw
    }
}