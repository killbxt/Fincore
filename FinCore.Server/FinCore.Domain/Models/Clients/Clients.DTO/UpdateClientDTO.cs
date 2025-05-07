using System.ComponentModel.DataAnnotations;

namespace FinCore.Domain.Models.Clients.Clients.DTO
{
    public class UpdateClientDTO
    {
        public required Guid Id { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [StringLength(30)]
        public string? Name { get; set; }

        [StringLength(30)]
        public string? Surname { get; set; }

        [StringLength(34)]
        public string? Patronymic { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Password { get; set; } 

        public DateOnly? DateOfBirth { get; set; }
    }
}
