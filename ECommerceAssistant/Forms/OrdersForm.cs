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

        public OrdersUserControl() { InitializeComponents(); LoadOrders(); }

        private void InitializeComponents()
        {
            var panelTop = new Panel { Dock = DockStyle.Top, Height = 90, BackColor = Color.FromArgb(250, 250, 250), Padding = new Padding(10) };
            var lblTitle = new Label { Text = "\u8ba2\u5355\u7ba1\u7406", Font = new Font("Microsoft YaHei UI", 12, FontStyle.Bold), AutoSize = true, Location = new Point(10, 10), ForeColor = Color.FromArgb(64, 64, 64) };
            lblSummary = new Label { Text = "", Font = new Font("Microsoft YaHei UI", 9), AutoSize = true, Location = new Point(120, 13), ForeColor = Color.FromArgb(100, 100, 100) };
            var lblStatus = new Label { Text = "\u72b6\u6001:", Font = new Font("Microsoft YaHei UI", 10), AutoSize = true, Location = new Point(10, 48) };
            cmbStatus = new ComboBox { Font = new Font("Microsoft YaHei UI", 10), Location = new Point(55, 45), Size = new Size(120, 25), DropDownStyle = ComboBoxStyle.DropDownList };
            cmbStatus.Items.AddRange(new object[] { "\u5168\u90e8", "\u5f85\u5904\u7406", "\u5df2\u5b8c\u6210", "\u5df2\u53d6\u6d88" });
            cmbStatus.SelectedIndex = 0;
            cmbStatus.SelectedIndexChanged += (s, e) => ApplyFilter();
            var lblSearch = new Label { Text = "\u8ba2\u5355\u53f7:", Font = new Font("Microsoft YaHei UI", 10), AutoSize = true, Location = new Point(200, 48) };
            txtSearch = new TextBox { Font = new Font("Microsoft YaHei UI", 10), Location = new Point(265, 45), Size = new Size(180, 25) };
            btnSearch = new Button { Text = "\u641c\u7d22", Font = new Font("Microsoft YaHei UI", 9), Size = new Size(70, 28), Location = new Point(455, 44), BackColor = Color.FromArgb(0, 122, 204), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand };
            btnSearch.Click += BtnSearch_Click;
            btnRefresh = new Button { Text = "\u5237\u65b0", Font = new Font("Microsoft YaHei UI", 9), Size = new Size(70, 28), Location = new Point(535, 44), BackColor = Color.FromArgb(108, 117, 125), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand };
            btnRefresh.Click += (s, e) => LoadOrders();
            panelTop.Controls.AddRange(new Control[] { lblTitle, lblSummary, lblStatus, cmbStatus, lblSearch, txtSearch, btnSearch, btnRefresh });

            dgvOrders = new DataGridView { Dock = DockStyle.Fill, AllowUserToAddRows = false, AllowUserToDeleteRows = false, ReadOnly = true, SelectionMode = DataGridViewSelectionMode.FullRowSelect, MultiSelect = false, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, BackgroundColor = Color.White, BorderStyle = BorderStyle.None, RowHeadersVisible = false, Font = new Font("Microsoft YaHei UI", 10) };
            dgvOrders.Columns.Add("Id", "ID");
            dgvOrders.Columns.Add("OrderNo", "\u8ba2\u5355\u53f7");
            dgvOrders.Columns.Add("ProductName", "\u5546\u54c1\u540d\u79f0");
            dgvOrders.Columns.Add("Quantity", "\u6570\u91cf");
            dgvOrders.Columns.Add("TotalAmount", "\u91d1\u989d");
            dgvOrders.Columns.Add("Status", "\u72b6\u6001");
            dgvOrders.Columns.Add("CreatedAt", "\u4e0b\u5355\u65f6\u95f4");
            dgvOrders.Columns["Id"].FillWeight = 8;
            dgvOrders.Columns["OrderNo"].FillWeight = 20;
            dgvOrders.Columns["ProductName"].FillWeight = 25;
            dgvOrders.Columns["Quantity"].FillWeight = 10;
            dgvOrders.Columns["TotalAmount"].FillWeight = 15;
            dgvOrders.Columns["Status"].FillWeight = 12;
            dgvOrders.Columns["CreatedAt"].FillWeight = 18;
            dgvOrders.CellFormatting += DgvOrders_CellFormatting;
            Controls.Add(dgvOrders);
            Controls.Add(panelTop);
        }

        private void LoadOrders() { PopulateGrid(DatabaseHelper.GetAllOrders()); }
        private void ApplyFilter()
        {
            var status = cmbStatus.SelectedItem != null ? cmbStatus.SelectedItem.ToString() : null;
            var searchText = txtSearch.Text.Trim();
            PopulateGrid(DatabaseHelper.GetAllOrders(status, string.IsNullOrWhiteSpace(searchText) ? null : searchText));
        }
        private void PopulateGrid(List<Order> orders)
        {
            dgvOrders.Rows.Clear();
            int totalOrders = orders.Count;
            decimal totalAmount = orders.Sum(o => o.TotalAmount);
            int pendingCount = orders.Count(o => o.Status == "\u5f85\u5904\u7406");
            foreach (var o in orders)
                dgvOrders.Rows.Add(o.Id, o.OrderNo, o.ProductName, o.Quantity, string.Format("\u00a5{0:F2}", o.TotalAmount), o.Status, o.CreatedAt.ToString("yyyy-MM-dd HH:mm"));
            lblSummary.Text = string.Format("\u5171 {0} \u7b14\u8ba2\u5355 | \u603b\u91d1\u989d: \u00a5{1:F2} | \u5f85\u5904\u7406: {2}", totalOrders, totalAmount, pendingCount);
        }

        private void DgvOrders_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvOrders.Columns[e.ColumnIndex].Name == "Status" && e.Value is string)
            {
                string status = (string)e.Value;
                if (status == "\u5f85\u5904\u7406") { e.CellStyle.ForeColor = Color.FromArgb(255, 152, 0); e.CellStyle.Font = new Font(dgvOrders.Font, FontStyle.Bold); }
                else if (status == "\u5df2\u5b8c\u6210") { e.CellStyle.ForeColor = Color.FromArgb(76, 175, 80); }
                else if (status == "\u5df2\u53d6\u6d88") { e.CellStyle.ForeColor = Color.Gray; }
            }
        }
        private void BtnSearch_Click(object sender, EventArgs e) { ApplyFilter(); }
    }
}