namespace TasksApi.Dto
{
    public class AuthDto
    {
        public string Name { get; set; } = String.Empty;
        public string Username { get; set; } = String.Empty;
        public required string Email { get; set; } = String.Empty;
        public required string Password { get; set; } = String.Empty;
        public string Address { get; set; } = String.Empty;
        public string Rol { get; set; } = String.Empty;
    }
}