using System;

namespace Loymax.Domain.Data.ModelsDto
{
    public class ClientDto : BaseEntityDto
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Patronymic { get; set; }
        public DateTime DateOfBirth { get; set; }
        public decimal AccountBalance { get; set; }
    }
}