using MinimalApi.Domain.Enuns;

namespace MinimalApi.Domain.DTOs
{
    public class AdminDTO
    {
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
        public Role? Role { get; set; } = default!;
    }
}
