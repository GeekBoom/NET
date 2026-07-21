using System;
namespace ECommerceAssistant.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string Category { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public Product() { Name = string.Empty; Category = string.Empty; ImageUrl = string.Empty; CreatedAt = DateTime.Now; }
    }
}