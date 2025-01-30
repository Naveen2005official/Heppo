namespace DatingAppAPI.DTOs
{
    public class UserDTO
    {
        public required string Username { get; set; }
        public required string Token { get; set; }
        public string? PhotoUrl { get; set; }
    }
}
