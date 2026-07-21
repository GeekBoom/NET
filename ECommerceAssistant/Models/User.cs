namespace ECommerceAssistant.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public User() { Username = string.Empty; Password = string.Empty; Role = "User"; }
    }
}