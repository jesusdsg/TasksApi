
namespace TasksApi.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = String.Empty;
        public string Username { get; set; } = String.Empty;
        public string Email { get; set; } = String.Empty;
        public string Password { get; set; } = String.Empty;
        public string Address { get; set; } = String.Empty;
        public string Rol { get; set; } = String.Empty;

    }
}