using System;

namespace Loymax.Domain.Data.ModelsDto
{
    public class BaseEntityDto
    {
        public Guid Id { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime? ModifyDateTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}