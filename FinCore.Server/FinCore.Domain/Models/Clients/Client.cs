using FinCore.Domain.Models.Cards;
using FinCore.Domain.Models.Clients.Clients.DTO;
using System.ComponentModel.DataAnnotations;

namespace FinCore.Domain.Models.Clients
{
    public class Client
    {
        [Key]
        public Guid Id { get; set; }

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

        public required string PhoneNumber { get; set; }

        public required string Password { get; set; }

        public DateOnly DateOfBirth { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? LastLoginDate { get; set; } 

        public ICollection<Card>? Cards { get; set; } = new List<Card>();

        public bool IsDeleted { get; set; } = false;

        public static explicit operator ClientDTO(Client client)
        {
            return new ClientDTO
            {
                Id = client.Id,
                Email = client.Email,
                Name = client.Name,
                Surname = client.Surname,
                Patronymic = client.Patronymic,
                PhoneNumber = client.PhoneNumber,
                DateOfBirth = client.DateOfBirth,
                LastLoginDate = client.LastLoginDate ?? DateTime.MinValue // Обработка null
            };
        }

        public static implicit operator Client(AddClientDTO addClientDTO)
        {
            return new Client
            {
                Email = addClientDTO.Email,
                Name = addClientDTO.Name,
                Surname = addClientDTO.Surname,
                Patronymic = addClientDTO.Patronymic,
                Password = addClientDTO.Password,
                PhoneNumber = addClientDTO.PhoneNumber,
                DateOfBirth = addClientDTO.DateOfBirth,
                LastLoginDate = DateTime.Now,
                CreatedAt = DateTime.Now,
                Id = Guid.NewGuid(),
            };
        }
        public static implicit operator Client(UpdateClientDTO updateClientDto)
        {
            return new Client
            {
                Id = updateClientDto.Id,
                Email = updateClientDto.Email,
                Name = updateClientDto.Name,
                Surname = updateClientDto.Surname,
                Patronymic = updateClientDto.Patronymic,
                Password = updateClientDto.Password,
                PhoneNumber = updateClientDto.PhoneNumber
            };
        }
    }
}
