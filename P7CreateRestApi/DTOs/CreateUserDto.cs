namespace P7CreateRestApi.DTOs
{
    public class CreateUserDto
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty; //mot de passe en clair uniquement en entrée
        public string Fullname { get; set; } = string.Empty;
        public string? Role { get; set; }
    }
}
