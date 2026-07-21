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
        public Order() { OrderNo = string.Empty; ProductName = string.Empty; Status = "\u5f85\u5904\u7406"; CreatedAt = DateTime.Now; }
    }
}