using System;
using System.ComponentModel.DataAnnotations;

namespace Loymax.Domain.Data.ModelsDto
{
    public class CreateClientDto
    {
        [Required]
        public string LastName { get; set; }
        [Required]
        public string FirstName { get; set; }
        public string Patronymic { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        public decimal AccountBalance { get; set; }
    }
}