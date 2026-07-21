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

            if (DatabaseHelper.HasData())
                return;

            // 添加默认用户
            DatabaseHelper.InsertUser(new User { Username = "admin", Password = "123456", Role = "Admin" });
            DatabaseHelper.InsertUser(new User { Username = "user1", Password = "123456", Role = "User" });

            // 添加示例商品
            var products = new List<Product>
            {
                new Product { Name = "iPhone 15 Pro", Price = 7999, Stock = 50, Category = "手机数码" },
                new Product { Name = "MacBook Air M3", Price = 9499, Stock = 30, Category = "电脑办公" },
                new Product { Name = "AirPods Pro 2", Price = 1899, Stock = 100, Category = "手机数码" },
                new Product { Name = "iPad Air 5", Price = 4799, Stock = 25, Category = "平板电脑" },
                new Product { Name = "Apple Watch S9", Price = 2999, Stock = 8, Category = "智能穿戴" },
                new Product { Name = "Sony WH-1000XM5", Price = 2499, Stock = 15, Category = "音频设备" },
                new Product { Name = "Samsung Galaxy S24", Price = 5999, Stock = 40, Category = "手机数码" },
                new Product { Name = "Dell XPS 15", Price = 11999, Stock = 5, Category = "电脑办公" },
                new Product { Name = "Nintendo Switch OLED", Price = 2499, Stock = 60, Category = "游戏娱乐" },
                new Product { Name = "Kindle Paperwhite", Price = 999, Stock = 3, Category = "电子阅读" }
            };

            foreach (var p in products)
            {
                DatabaseHelper.InsertProductRaw(p);
            }

            // 添加示例订单
            var orders = new List<Order>
            {
                new Order { OrderNo = "ORD20260701001", ProductId = 1, ProductName = "iPhone 15 Pro", Quantity = 2, TotalAmount = 15998, Status = "已完成", CreatedAt = DateTime.Now.AddDays(-20) },
                new Order { OrderNo = "ORD20260705002", ProductId = 2, ProductName = "MacBook Air M3", Quantity = 1, TotalAmount = 9499, Status = "已完成", CreatedAt = DateTime.Now.AddDays(-16) },
                new Order { OrderNo = "ORD20260710003", ProductId = 3, ProductName = "AirPods Pro 2", Quantity = 3, TotalAmount = 5697, Status = "待处理", CreatedAt = DateTime.Now.AddDays(-11) },
                new Order { OrderNo = "ORD20260715004", ProductId = 5, ProductName = "Apple Watch S9", Quantity = 1, TotalAmount = 2999, Status = "待处理", CreatedAt = DateTime.Now.AddDays(-6) },
                new Order { OrderNo = "ORD20260718005", ProductId = 7, ProductName = "Samsung Galaxy S24", Quantity = 2, TotalAmount = 11998, Status = "已取消", CreatedAt = DateTime.Now.AddDays(-3) },
                new Order { OrderNo = "ORD20260720006", ProductId = 9, ProductName = "Nintendo Switch OLED", Quantity = 1, TotalAmount = 2499, Status = "待处理", CreatedAt = DateTime.Now.AddDays(-1) }
            };

            foreach (var o in orders)
            {
                DatabaseHelper.InsertOrderRaw(o);
            }
        }
    }
}
using ECommerceAssistant.Data;
using ECommerceAssistant.Models;

namespace ECommerceAssistant;

public static class SeedData
{
    public static void Initialize()
    {
        using var context = new AppDbContext();
        context.Database.EnsureCreated();

        // 检查是否已有数据
        if (context.Users.Any())
            return;

        // 添加默认用户
        context.Users.Add(new User
        {
            Username = "admin",
            Password = "123456",
            Role = "Admin"
        });
        context.Users.Add(new User
        {
            Username = "user1",
            Password = "123456",
            Role = "User"
        });

        // 添加示例商品
        var products = new List<Product>
        {
            new() { Name = "iPhone 15 Pro", Price = 7999, Stock = 50, Category = "手机数码" },
            new() { Name = "MacBook Air M3", Price = 9499, Stock = 30, Category = "电脑办公" },
            new() { Name = "AirPods Pro 2", Price = 1899, Stock = 100, Category = "手机数码" },
            new() { Name = "iPad Air 5", Price = 4799, Stock = 25, Category = "平板电脑" },
            new() { Name = "Apple Watch S9", Price = 2999, Stock = 8, Category = "智能穿戴" },
            new() { Name = "Sony WH-1000XM5", Price = 2499, Stock = 15, Category = "音频设备" },
            new() { Name = "Samsung Galaxy S24", Price = 5999, Stock = 40, Category = "手机数码" },
            new() { Name = "Dell XPS 15", Price = 11999, Stock = 5, Category = "电脑办公" },
            new() { Name = "Nintendo Switch OLED", Price = 2499, Stock = 60, Category = "游戏娱乐" },
            new() { Name = "Kindle Paperwhite", Price = 999, Stock = 3, Category = "电子阅读" }
        };
        context.Products.AddRange(products);

        // 添加示例订单
        var orders = new List<Order>
        {
            new() { OrderNo = "ORD20260701001", ProductId = 1, ProductName = "iPhone 15 Pro", Quantity = 2, TotalAmount = 15998, Status = "已完成", CreatedAt = DateTime.Now.AddDays(-20) },
            new() { OrderNo = "ORD20260705002", ProductId = 2, ProductName = "MacBook Air M3", Quantity = 1, TotalAmount = 9499, Status = "已完成", CreatedAt = DateTime.Now.AddDays(-16) },
            new() { OrderNo = "ORD20260710003", ProductId = 3, ProductName = "AirPods Pro 2", Quantity = 3, TotalAmount = 5697, Status = "待处理", CreatedAt = DateTime.Now.AddDays(-11) },
            new() { OrderNo = "ORD20260715004", ProductId = 5, ProductName = "Apple Watch S9", Quantity = 1, TotalAmount = 2999, Status = "待处理", CreatedAt = DateTime.Now.AddDays(-6) },
            new() { OrderNo = "ORD20260718005", ProductId = 7, ProductName = "Samsung Galaxy S24", Quantity = 2, TotalAmount = 11998, Status = "已取消", CreatedAt = DateTime.Now.AddDays(-3) },
            new() { OrderNo = "ORD20260720006", ProductId = 9, ProductName = "Nintendo Switch OLED", Quantity = 1, TotalAmount = 2499, Status = "待处理", CreatedAt = DateTime.Now.AddDays(-1) }
        };
        context.Orders.AddRange(orders);

        context.SaveChanges();
    }
}
