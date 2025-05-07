using System.ComponentModel.DataAnnotations;

namespace FinCore.Domain.Models.Clients.Clients.DTO
{
    public class AddClientDTO
    {
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [StringLength(30)]
        public required string Name { get; set; }

        [Required]
        [StringLength(30)]
        public required string Surname { get; set; }

        [StringLength(34)]
        public string? Patronymic { get; set; }

        [Required]
        public required string PhoneNumber { get; set; }

        [Required]
        public required string Password { get; set; }

        public required DateOnly DateOfBirth { get; set; }
    }
}
