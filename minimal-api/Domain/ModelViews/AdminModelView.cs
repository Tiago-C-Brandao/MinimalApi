using MinimalApi.Domain.Enuns;

namespace MinimalApi.Domain.ModelViews
{
    public record AdminModelView
    {
        public int id { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }
}
