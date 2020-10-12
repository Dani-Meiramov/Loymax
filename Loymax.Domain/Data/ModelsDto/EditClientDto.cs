using System;

namespace Loymax.Domain.Data.ModelsDto
{
    public class EditClientDto : CreateClientDto
    {
        public Guid Id { get; set; }
    }
}