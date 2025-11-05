using MinimalApi.Domain.Enuns;

namespace MinimalApi.Domain.ModelViews
{
    public record AdminLoggedIn
    {
        public string Email { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }
    }
}
