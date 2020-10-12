using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Loymax.Domain.Data.Models
{
    public class Client : BaseEntity
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Patronymic { get; set; }
        public DateTime DateOfBirth { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal AccountBalance { get; set; }
    }
}