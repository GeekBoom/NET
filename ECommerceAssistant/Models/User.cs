namespace ECommerceAssistant.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }

        public User()
        {
            Username = string.Empty;
            Password = string.Empty;
            Role = "User";
        }
    }
}
using System.ComponentModel.DataAnnotations;

namespace ECommerceAssistant.Models;

public class User
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Password { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Role { get; set; } = "User";
}
