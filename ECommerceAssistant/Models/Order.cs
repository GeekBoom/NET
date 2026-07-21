using System;

namespace ECommerceAssistant.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string OrderNo { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }

        public Order()
        {
            OrderNo = string.Empty;
            ProductName = string.Empty;
            Status = "待处理";
            CreatedAt = DateTime.Now;
        }
    }
}
using System.ComponentModel.DataAnnotations;

namespace ECommerceAssistant.Models;

public class Order
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string OrderNo { get; set; } = string.Empty;

    public int ProductId { get; set; }

    [Required]
    [MaxLength(100)]
    public string ProductName { get; set; } = string.Empty;

    public int Quantity { get; set; }

    public decimal TotalAmount { get; set; }

    [MaxLength(20)]
    public string Status { get; set; } = "待处理"; // 待处理 / 已完成 / 已取消

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
