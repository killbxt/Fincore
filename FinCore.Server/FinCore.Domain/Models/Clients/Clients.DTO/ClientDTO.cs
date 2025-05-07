namespace FinCore.Domain.Models.Clients.Clients.DTO
{
    public class ClientDTO
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string? Patronymic { get; set; }
        public string PhoneNumber { get; set; } = null!;
        public DateOnly DateOfBirth { get; set; }
        public DateTime LastLoginDate { get; set; }
    }
}
