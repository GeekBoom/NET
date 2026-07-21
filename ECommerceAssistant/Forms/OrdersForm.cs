using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ECommerceAssistant.Data;
using ECommerceAssistant.Models;

namespace ECommerceAssistant.Forms
{
    public class OrdersUserControl : UserControl
    {
        private DataGridView dgvOrders;
        private TextBox txtSearch;
        private ComboBox cmbStatus;
        private Button btnSearch;
        private Button btnRefresh;
        private Label lblSummary;

        public OrdersUserControl()
        {
            InitializeComponents();
            LoadOrders();
        }

        private void InitializeComponents()
        {
            // 顶部工具栏
            var panelTop = new Panel
            {
                Dock = DockStyle.Top,
                Height = 90,
                BackColor = Color.FromArgb(250, 250, 250),
                Padding = new Padding(10)
            };

            var lblTitle = new Label
            {
                Text = "订单管理",
                Font = new Font("Microsoft YaHei UI", 12, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(10, 10),
                ForeColor = Color.FromArgb(64, 64, 64)
            };

            lblSummary = new Label
            {
                Text = "",
                Font = new Font("Microsoft YaHei UI", 9),
                AutoSize = true,
                Location = new Point(120, 13),
                ForeColor = Color.FromArgb(100, 100, 100)
            };

            // 第二行：状态筛选
            var lblStatus = new Label
            {
                Text = "状态:",
                Font = new Font("Microsoft YaHei UI", 10),
                AutoSize = true,
                Location = new Point(10, 48)
            };

            cmbStatus = new ComboBox
            {
                Font = new Font("Microsoft YaHei UI", 10),
                Location = new Point(55, 45),
                Size = new Size(120, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbStatus.Items.AddRange(new object[] { "全部", "待处理", "已完成", "已取消" });
            cmbStatus.SelectedIndex = 0;
            cmbStatus.SelectedIndexChanged += (s, e) => ApplyFilter();

            // 搜索框
            var lblSearch = new Label
            {
                Text = "订单号:",
                Font = new Font("Microsoft YaHei UI", 10),
                AutoSize = true,
                Location = new Point(200, 48)
            };

            txtSearch = new TextBox
            {
                Font = new Font("Microsoft YaHei UI", 10),
                Location = new Point(265, 45),
                Size = new Size(180, 25)
            };

            btnSearch = new Button
            {
                Text = "搜索",
                Font = new Font("Microsoft YaHei UI", 9),
                Size = new Size(70, 28),
                Location = new Point(455, 44),
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnSearch.Click += BtnSearch_Click;

            btnRefresh = new Button
            {
                Text = "刷新",
                Font = new Font("Microsoft YaHei UI", 9),
                Size = new Size(70, 28),
                Location = new Point(535, 44),
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnRefresh.Click += (s, e) => LoadOrders();

            panelTop.Controls.AddRange(new Control[] { lblTitle, lblSummary, lblStatus, cmbStatus, lblSearch, txtSearch, btnSearch, btnRefresh });

            // 数据表格
            dgvOrders = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                Font = new Font("Microsoft YaHei UI", 10)
            };

            dgvOrders.Columns.Add("Id", "ID");
            dgvOrders.Columns.Add("OrderNo", "订单号");
            dgvOrders.Columns.Add("ProductName", "商品名称");
            dgvOrders.Columns.Add("Quantity", "数量");
            dgvOrders.Columns.Add("TotalAmount", "金额");
            dgvOrders.Columns.Add("Status", "状态");
            dgvOrders.Columns.Add("CreatedAt", "下单时间");

            dgvOrders.Columns["Id"].FillWeight = 8;
            dgvOrders.Columns["OrderNo"].FillWeight = 20;
            dgvOrders.Columns["ProductName"].FillWeight = 25;
            dgvOrders.Columns["Quantity"].FillWeight = 10;
            dgvOrders.Columns["TotalAmount"].FillWeight = 15;
            dgvOrders.Columns["Status"].FillWeight = 12;
            dgvOrders.Columns["CreatedAt"].FillWeight = 18;

            // 行格式化
            dgvOrders.CellFormatting += DgvOrders_CellFormatting;

            // 添加到控件
            Controls.Add(dgvOrders);
            Controls.Add(panelTop);
        }

        private void LoadOrders()
        {
            var orders = DatabaseHelper.GetAllOrders();
            PopulateGrid(orders);
        }

        private void ApplyFilter()
        {
            var status = cmbStatus.SelectedItem != null ? cmbStatus.SelectedItem.ToString() : null;
            var searchText = txtSearch.Text.Trim();

            var orders = DatabaseHelper.GetAllOrders(status, string.IsNullOrWhiteSpace(searchText) ? null : searchText);
            PopulateGrid(orders);
        }

        private void PopulateGrid(List<Order> orders)
        {
            dgvOrders.Rows.Clear();

            int totalOrders = orders.Count;
            decimal totalAmount = orders.Sum(o => o.TotalAmount);
            int pendingCount = orders.Count(o => o.Status == "待处理");

            foreach (var o in orders)
            {
                dgvOrders.Rows.Add(o.Id, o.OrderNo, o.ProductName, o.Quantity,
                    string.Format("¥{0:F2}", o.TotalAmount), o.Status, o.CreatedAt.ToString("yyyy-MM-dd HH:mm"));
            }

            lblSummary.Text = string.Format("共 {0} 笔订单 | 总金额: ¥{1:F2} | 待处理: {2}", totalOrders, totalAmount, pendingCount);
        }

        private void DgvOrders_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvOrders.Columns[e.ColumnIndex].Name == "Status" && e.Value is string)
            {
                string status = (string)e.Value;
                switch (status)
                {
                    case "待处理":
                        e.CellStyle.ForeColor = Color.FromArgb(255, 152, 0);
                        e.CellStyle.Font = new Font(dgvOrders.Font, FontStyle.Bold);
                        break;
                    case "已完成":
                        e.CellStyle.ForeColor = Color.FromArgb(76, 175, 80);
                        break;
                    case "已取消":
                        e.CellStyle.ForeColor = Color.Gray;
                        break;
                }
            }
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            ApplyFilter();
        }
    }
}
using ECommerceAssistant.Data;
using ECommerceAssistant.Models;

namespace ECommerceAssistant.Forms;

public class OrdersUserControl : UserControl
{
    private DataGridView dgvOrders = null!;
    private TextBox txtSearch = null!;
    private ComboBox cmbStatus = null!;
    private Button btnSearch = null!;
    private Button btnRefresh = null!;
    private Label lblSummary = null!;

    public OrdersUserControl()
    {
        InitializeComponents();
        LoadOrders();
    }

    private void InitializeComponents()
    {
        // 顶部工具栏
        var panelTop = new Panel
        {
            Dock = DockStyle.Top,
            Height = 90,
            BackColor = Color.FromArgb(250, 250, 250),
            Padding = new Padding(10)
        };

        var lblTitle = new Label
        {
            Text = "订单管理",
            Font = new Font("Microsoft YaHei UI", 12, FontStyle.Bold),
            AutoSize = true,
            Location = new Point(10, 10),
            ForeColor = Color.FromArgb(64, 64, 64)
        };

        lblSummary = new Label
        {
            Text = "",
            Font = new Font("Microsoft YaHei UI", 9),
            AutoSize = true,
            Location = new Point(120, 13),
            ForeColor = Color.FromArgb(100, 100, 100)
        };

        // 第二行：状态筛选
        var lblStatus = new Label
        {
            Text = "状态:",
            Font = new Font("Microsoft YaHei UI", 10),
            AutoSize = true,
            Location = new Point(10, 48)
        };

        cmbStatus = new ComboBox
        {
            Font = new Font("Microsoft YaHei UI", 10),
            Location = new Point(55, 45),
            Size = new Size(120, 25),
            DropDownStyle = ComboBoxStyle.DropDownList
        };
        cmbStatus.Items.AddRange(new object[] { "全部", "待处理", "已完成", "已取消" });
        cmbStatus.SelectedIndex = 0;
        cmbStatus.SelectedIndexChanged += (s, e) => ApplyFilter();

        // 搜索框
        var lblSearch = new Label
        {
            Text = "订单号:",
            Font = new Font("Microsoft YaHei UI", 10),
            AutoSize = true,
            Location = new Point(200, 48)
        };

        txtSearch = new TextBox
        {
            Font = new Font("Microsoft YaHei UI", 10),
            Location = new Point(265, 45),
            Size = new Size(180, 25)
        };

        btnSearch = new Button
        {
            Text = "搜索",
            Font = new Font("Microsoft YaHei UI", 9),
            Size = new Size(70, 28),
            Location = new Point(455, 44),
            BackColor = Color.FromArgb(0, 122, 204),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand
        };
        btnSearch.Click += BtnSearch_Click;

        btnRefresh = new Button
        {
            Text = "刷新",
            Font = new Font("Microsoft YaHei UI", 9),
            Size = new Size(70, 28),
            Location = new Point(535, 44),
            BackColor = Color.FromArgb(108, 117, 125),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand
        };
        btnRefresh.Click += (s, e) => LoadOrders();

        panelTop.Controls.AddRange(new Control[] { lblTitle, lblSummary, lblStatus, cmbStatus, lblSearch, txtSearch, btnSearch, btnRefresh });

        // 数据表格
        dgvOrders = new DataGridView
        {
            Dock = DockStyle.Fill,
            AllowUserToAddRows = false,
            AllowUserToDeleteRows = false,
            ReadOnly = true,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            MultiSelect = false,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            BackgroundColor = Color.White,
            BorderStyle = BorderStyle.None,
            RowHeadersVisible = false,
            Font = new Font("Microsoft YaHei UI", 10)
        };

        dgvOrders.Columns.Add("Id", "ID");
        dgvOrders.Columns.Add("OrderNo", "订单号");
        dgvOrders.Columns.Add("ProductName", "商品名称");
        dgvOrders.Columns.Add("Quantity", "数量");
        dgvOrders.Columns.Add("TotalAmount", "金额");
        dgvOrders.Columns.Add("Status", "状态");
        dgvOrders.Columns.Add("CreatedAt", "下单时间");

        dgvOrders.Columns["Id"]!.FillWeight = 8;
        dgvOrders.Columns["OrderNo"]!.FillWeight = 20;
        dgvOrders.Columns["ProductName"]!.FillWeight = 25;
        dgvOrders.Columns["Quantity"]!.FillWeight = 10;
        dgvOrders.Columns["TotalAmount"]!.FillWeight = 15;
        dgvOrders.Columns["Status"]!.FillWeight = 12;
        dgvOrders.Columns["CreatedAt"]!.FillWeight = 18;

        // 行格式化
        dgvOrders.CellFormatting += DgvOrders_CellFormatting;

        // 添加到控件
        Controls.Add(dgvOrders);
        Controls.Add(panelTop);
    }

    private void LoadOrders()
    {
        using var context = new AppDbContext();
        var orders = context.Orders.OrderByDescending(o => o.CreatedAt).ToList();
        PopulateGrid(orders);
    }

    private void ApplyFilter()
    {
        using var context = new AppDbContext();
        var query = context.Orders.AsQueryable();

        var status = cmbStatus.SelectedItem?.ToString();
        if (!string.IsNullOrEmpty(status) && status != "全部")
        {
            query = query.Where(o => o.Status == status);
        }

        var searchText = txtSearch.Text.Trim();
        if (!string.IsNullOrWhiteSpace(searchText))
        {
            query = query.Where(o => o.OrderNo.Contains(searchText));
        }

        var orders = query.OrderByDescending(o => o.CreatedAt).ToList();
        PopulateGrid(orders);
    }

    private void PopulateGrid(List<Order> orders)
    {
        dgvOrders.Rows.Clear();

        int totalOrders = orders.Count;
        decimal totalAmount = orders.Sum(o => o.TotalAmount);
        int pendingCount = orders.Count(o => o.Status == "待处理");

        foreach (var o in orders)
        {
            dgvOrders.Rows.Add(o.Id, o.OrderNo, o.ProductName, o.Quantity,
                $"¥{o.TotalAmount:F2}", o.Status, o.CreatedAt.ToString("yyyy-MM-dd HH:mm"));
        }

        lblSummary.Text = $"共 {totalOrders} 笔订单 | 总金额: ¥{totalAmount:F2} | 待处理: {pendingCount}";
    }

    private void DgvOrders_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        if (dgvOrders.Columns[e.ColumnIndex].Name == "Status" && e.Value is string status)
        {
            switch (status)
            {
                case "待处理":
                    e.CellStyle.ForeColor = Color.FromArgb(255, 152, 0);
                    e.CellStyle.Font = new Font(dgvOrders.Font, FontStyle.Bold);
                    break;
                case "已完成":
                    e.CellStyle.ForeColor = Color.FromArgb(76, 175, 80);
                    break;
                case "已取消":
                    e.CellStyle.ForeColor = Color.Gray;
                    break;
            }
        }
    }

    private void BtnSearch_Click(object? sender, EventArgs e)
    {
        ApplyFilter();
    }
}
