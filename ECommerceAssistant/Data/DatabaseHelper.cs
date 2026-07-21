using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using ECommerceAssistant.Models;

namespace ECommerceAssistant.Data
{
    public static class DatabaseHelper
    {
        private static readonly string DbPath;
        private static readonly string ConnectionString;

        static DatabaseHelper()
        {
            var appDataPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "ECommerceAssistant");
            Directory.CreateDirectory(appDataPath);
            DbPath = Path.Combine(appDataPath, "ecommerce.db");
            ConnectionString = $"Data Source={DbPath};Version=3;";
        }

        public static void InitializeDatabase()
        {
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();

                // 创建 Users 表
                var cmd = conn.CreateCommand();
                cmd.CommandText = @"
                    CREATE TABLE IF NOT EXISTS Users (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Username TEXT NOT NULL,
                        Password TEXT NOT NULL,
                        Role TEXT DEFAULT 'User'
                    )";
                cmd.ExecuteNonQuery();

                // 创建 Products 表
                cmd.CommandText = @"
                    CREATE TABLE IF NOT EXISTS Products (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name TEXT NOT NULL,
                        Price REAL NOT NULL,
                        Stock INTEGER NOT NULL DEFAULT 0,
                        Category TEXT DEFAULT '',
                        ImageUrl TEXT DEFAULT '',
                        CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP
                    )";
                cmd.ExecuteNonQuery();

                // 创建 Orders 表
                cmd.CommandText = @"
                    CREATE TABLE IF NOT EXISTS Orders (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        OrderNo TEXT NOT NULL,
                        ProductId INTEGER NOT NULL,
                        ProductName TEXT NOT NULL,
                        Quantity INTEGER NOT NULL,
                        TotalAmount REAL NOT NULL,
                        Status TEXT DEFAULT '待处理',
                        CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP
                    )";
                cmd.ExecuteNonQuery();
            }
        }

        public static User FindUser(string username, string password)
        {
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT Id, Username, Password, Role FROM Users WHERE Username = @username AND Password = @password";
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", password);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new User
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Username = reader["Username"].ToString(),
                            Password = reader["Password"].ToString(),
                            Role = reader["Role"].ToString()
                        };
                    }
                }
            }
            return null;
        }

        public static List<Product> GetAllProducts(string searchText = null)
        {
            var products = new List<Product>();
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                var cmd = conn.CreateCommand();

                if (!string.IsNullOrWhiteSpace(searchText))
                {
                    cmd.CommandText = "SELECT * FROM Products WHERE Name LIKE @search OR Category LIKE @search ORDER BY CreatedAt DESC";
                    cmd.Parameters.AddWithValue("@search", "%" + searchText + "%");
                }
                else
                {
                    cmd.CommandText = "SELECT * FROM Products ORDER BY CreatedAt DESC";
                }

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        products.Add(new Product
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Name = reader["Name"].ToString(),
                            Price = Convert.ToDecimal(reader["Price"]),
                            Stock = Convert.ToInt32(reader["Stock"]),
                            Category = reader["Category"].ToString(),
                            ImageUrl = reader["ImageUrl"]?.ToString(),
                            CreatedAt = Convert.ToDateTime(reader["CreatedAt"])
                        });
                    }
                }
            }
            return products;
        }

        public static Product GetProductById(int id)
        {
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT * FROM Products WHERE Id = @id";
                cmd.Parameters.AddWithValue("@id", id);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Product
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Name = reader["Name"].ToString(),
                            Price = Convert.ToDecimal(reader["Price"]),
                            Stock = Convert.ToInt32(reader["Stock"]),
                            Category = reader["Category"].ToString(),
                            ImageUrl = reader["ImageUrl"]?.ToString(),
                            CreatedAt = Convert.ToDateTime(reader["CreatedAt"])
                        };
                    }
                }
            }
            return null;
        }

        public static void InsertProduct(Product product)
        {
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText = @"INSERT INTO Products (Name, Price, Stock, Category, ImageUrl, CreatedAt) 
                                    VALUES (@name, @price, @stock, @category, @imageUrl, @createdAt)";
                cmd.Parameters.AddWithValue("@name", product.Name);
                cmd.Parameters.AddWithValue("@price", product.Price);
                cmd.Parameters.AddWithValue("@stock", product.Stock);
                cmd.Parameters.AddWithValue("@category", product.Category);
                cmd.Parameters.AddWithValue("@imageUrl", (object)product.ImageUrl ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@createdAt", product.CreatedAt);
                cmd.ExecuteNonQuery();
            }
        }

        public static void UpdateProduct(Product product)
        {
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText = @"UPDATE Products SET Name = @name, Price = @price, Stock = @stock, 
                                    Category = @category, ImageUrl = @imageUrl WHERE Id = @id";
                cmd.Parameters.AddWithValue("@id", product.Id);
                cmd.Parameters.AddWithValue("@name", product.Name);
                cmd.Parameters.AddWithValue("@price", product.Price);
                cmd.Parameters.AddWithValue("@stock", product.Stock);
                cmd.Parameters.AddWithValue("@category", product.Category);
                cmd.Parameters.AddWithValue("@imageUrl", (object)product.ImageUrl ?? DBNull.Value);
                cmd.ExecuteNonQuery();
            }
        }

        public static List<Product> GetAllProductsOrderByStock()
        {
            var products = new List<Product>();
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT * FROM Products ORDER BY Stock ASC";

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        products.Add(new Product
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Name = reader["Name"].ToString(),
                            Price = Convert.ToDecimal(reader["Price"]),
                            Stock = Convert.ToInt32(reader["Stock"]),
                            Category = reader["Category"].ToString(),
                            ImageUrl = reader["ImageUrl"]?.ToString(),
                            CreatedAt = Convert.ToDateTime(reader["CreatedAt"])
                        });
                    }
                }
            }
            return products;
        }

        public static void UpdateProductStock(int id, int stock)
        {
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText = "UPDATE Products SET Stock = @stock WHERE Id = @id";
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@stock", stock);
                cmd.ExecuteNonQuery();
            }
        }

        public static List<Order> GetAllOrders(string statusFilter = null, string searchText = null)
        {
            var orders = new List<Order>();
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                var cmd = conn.CreateCommand();

                string whereClause = "";
                if (!string.IsNullOrWhiteSpace(statusFilter) && statusFilter != "全部")
                {
                    whereClause += " AND Status = @status";
                    cmd.Parameters.AddWithValue("@status", statusFilter);
                }
                if (!string.IsNullOrWhiteSpace(searchText))
                {
                    whereClause += " AND OrderNo LIKE @search";
                    cmd.Parameters.AddWithValue("@search", "%" + searchText + "%");
                }

                cmd.CommandText = "SELECT * FROM Orders WHERE 1=1 " + whereClause + " ORDER BY CreatedAt DESC";

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        orders.Add(new Order
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            OrderNo = reader["OrderNo"].ToString(),
                            ProductId = Convert.ToInt32(reader["ProductId"]),
                            ProductName = reader["ProductName"].ToString(),
                            Quantity = Convert.ToInt32(reader["Quantity"]),
                            TotalAmount = Convert.ToDecimal(reader["TotalAmount"]),
                            Status = reader["Status"].ToString(),
                            CreatedAt = Convert.ToDateTime(reader["CreatedAt"])
                        });
                    }
                }
            }
            return orders;
        }

        public static bool HasData()
        {
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT COUNT(*) FROM Users";
                var count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0;
            }
        }

        public static void InsertUser(User user)
        {
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText = "INSERT INTO Users (Username, Password, Role) VALUES (@username, @password, @role)";
                cmd.Parameters.AddWithValue("@username", user.Username);
                cmd.Parameters.AddWithValue("@password", user.Password);
                cmd.Parameters.AddWithValue("@role", user.Role);
                cmd.ExecuteNonQuery();
            }
        }

        public static void InsertProductRaw(Product product)
        {
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText = @"INSERT INTO Products (Name, Price, Stock, Category, CreatedAt) 
                                    VALUES (@name, @price, @stock, @category, @createdAt)";
                cmd.Parameters.AddWithValue("@name", product.Name);
                cmd.Parameters.AddWithValue("@price", product.Price);
                cmd.Parameters.AddWithValue("@stock", product.Stock);
                cmd.Parameters.AddWithValue("@category", product.Category);
                cmd.Parameters.AddWithValue("@createdAt", product.CreatedAt);
                cmd.ExecuteNonQuery();
            }
        }

        public static void InsertOrderRaw(Order order)
        {
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText = @"INSERT INTO Orders (OrderNo, ProductId, ProductName, Quantity, TotalAmount, Status, CreatedAt) 
                                    VALUES (@orderNo, @productId, @productName, @quantity, @totalAmount, @status, @createdAt)";
                cmd.Parameters.AddWithValue("@orderNo", order.OrderNo);
                cmd.Parameters.AddWithValue("@productId", order.ProductId);
                cmd.Parameters.AddWithValue("@productName", order.ProductName);
                cmd.Parameters.AddWithValue("@quantity", order.Quantity);
                cmd.Parameters.AddWithValue("@totalAmount", order.TotalAmount);
                cmd.Parameters.AddWithValue("@status", order.Status);
                cmd.Parameters.AddWithValue("@createdAt", order.CreatedAt);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
