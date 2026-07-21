using System;
using System.Collections.Generic;
using ECommerceAssistant.Data;
using ECommerceAssistant.Models;

namespace ECommerceAssistant
{
    public static class SeedData
    {
        public static void Initialize()
        {
            DatabaseHelper.InitializeDatabase();
            if (DatabaseHelper.HasData()) return;
            DatabaseHelper.InsertUser(new User { Username = "admin", Password = "123456", Role = "Admin" });
            DatabaseHelper.InsertUser(new User { Username = "user1", Password = "123456", Role = "User" });
            var products = new List<Product>
            {
                new Product { Name = "iPhone 15 Pro", Price = 7999, Stock = 50, Category = "\u624b\u673a\u6570\u7801" },
                new Product { Name = "MacBook Air M3", Price = 9499, Stock = 30, Category = "\u7535\u8111\u529e\u516c" },
                new Product { Name = "AirPods Pro 2", Price = 1899, Stock = 100, Category = "\u624b\u673a\u6570\u7801" },
                new Product { Name = "iPad Air 5", Price = 4799, Stock = 25, Category = "\u5e73\u677f\u7535\u8111" },
                new Product { Name = "Apple Watch S9", Price = 2999, Stock = 8, Category = "\u667a\u80fd\u7a7f\u6234" },
                new Product { Name = "Sony WH-1000XM5", Price = 2499, Stock = 15, Category = "\u97f3\u9891\u8bbe\u5907" },
                new Product { Name = "Samsung Galaxy S24", Price = 5999, Stock = 40, Category = "\u624b\u673a\u6570\u7801" },
                new Product { Name = "Dell XPS 15", Price = 11999, Stock = 5, Category = "\u7535\u8111\u529e\u516c" },
                new Product { Name = "Nintendo Switch OLED", Price = 2499, Stock = 60, Category = "\u6e38\u620f\u5a31\u4e50" },
                new Product { Name = "Kindle Paperwhite", Price = 999, Stock = 3, Category = "\u7535\u5b50\u9605\u8bfb" }
            };
            foreach (var p in products) DatabaseHelper.InsertProductRaw(p);
            var orders = new List<Order>
            {
                new Order { OrderNo = "ORD20260701001", ProductId = 1, ProductName = "iPhone 15 Pro", Quantity = 2, TotalAmount = 15998, Status = "\u5df2\u5b8c\u6210", CreatedAt = DateTime.Now.AddDays(-20) },
                new Order { OrderNo = "ORD20260705002", ProductId = 2, ProductName = "MacBook Air M3", Quantity = 1, TotalAmount = 9499, Status = "\u5df2\u5b8c\u6210", CreatedAt = DateTime.Now.AddDays(-16) },
                new Order { OrderNo = "ORD20260710003", ProductId = 3, ProductName = "AirPods Pro 2", Quantity = 3, TotalAmount = 5697, Status = "\u5f85\u5904\u7406", CreatedAt = DateTime.Now.AddDays(-11) },
                new Order { OrderNo = "ORD20260715004", ProductId = 5, ProductName = "Apple Watch S9", Quantity = 1, TotalAmount = 2999, Status = "\u5f85\u5904\u7406", CreatedAt = DateTime.Now.AddDays(-6) },
                new Order { OrderNo = "ORD20260718005", ProductId = 7, ProductName = "Samsung Galaxy S24", Quantity = 2, TotalAmount = 11998, Status = "\u5df2\u53d6\u6d88", CreatedAt = DateTime.Now.AddDays(-3) },
                new Order { OrderNo = "ORD20260720006", ProductId = 9, ProductName = "Nintendo Switch OLED", Quantity = 1, TotalAmount = 2499, Status = "\u5f85\u5904\u7406", CreatedAt = DateTime.Now.AddDays(-1) }
            };
            foreach (var o in orders) DatabaseHelper.InsertOrderRaw(o);
        }
    }
}