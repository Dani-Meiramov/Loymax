using System;

namespace Loymax.Domain.Data.Models
{
    public class BaseEntity
    {
        public Guid Id { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime? ModifyDateTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}