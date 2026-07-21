using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ECommerceAssistant.Data;
using ECommerceAssistant.Models;

namespace ECommerceAssistant.Forms
{
    public class ProductsUserControl : UserControl
    {
        private DataGridView dgvProducts;
        private TextBox txtSearch;
        private Button btnAdd;
        private Button btnEdit;
        private Button btnRefresh;
        private Button btnSearch;

        public ProductsUserControl()
        {
            InitializeComponents();
            LoadProducts();
        }

        private void InitializeComponents()
        {
            var panelTop = new Panel { Dock = DockStyle.Top, Height = 50, BackColor = Color.FromArgb(250, 250, 250), Padding = new Padding(10) };
            var lblSearch = new Label { Text = "\u641c\u7d22:", Font = new Font("Microsoft YaHei UI", 10), AutoSize = true, Location = new Point(10, 14) };
            txtSearch = new TextBox { Font = new Font("Microsoft YaHei UI", 10), Location = new Point(55, 11), Size = new Size(200, 25) };
            btnSearch = new Button { Text = "\u641c\u7d22", Font = new Font("Microsoft YaHei UI", 9), Size = new Size(70, 28), Location = new Point(265, 10), BackColor = Color.FromArgb(0, 122, 204), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand };
            btnSearch.Click += BtnSearch_Click;
            btnAdd = new Button { Text = "\u65b0\u589e\u5546\u54c1", Font = new Font("Microsoft YaHei UI", 9), Size = new Size(90, 28), Location = new Point(350, 10), BackColor = Color.FromArgb(40, 167, 69), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand };
            btnAdd.Click += BtnAdd_Click;
            btnEdit = new Button { Text = "\u7f16\u8f91\u5546\u54c1", Font = new Font("Microsoft YaHei UI", 9), Size = new Size(90, 28), Location = new Point(450, 10), BackColor = Color.FromArgb(255, 193, 7), ForeColor = Color.Black, FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand };
            btnEdit.Click += BtnEdit_Click;
            btnRefresh = new Button { Text = "\u5237\u65b0", Font = new Font("Microsoft YaHei UI", 9), Size = new Size(70, 28), Location = new Point(550, 10), BackColor = Color.FromArgb(108, 117, 125), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand };
            btnRefresh.Click += (s, e) => LoadProducts();
            panelTop.Controls.AddRange(new Control[] { lblSearch, txtSearch, btnSearch, btnAdd, btnEdit, btnRefresh });

            dgvProducts = new DataGridView
            {
                Dock = DockStyle.Fill, AllowUserToAddRows = false, AllowUserToDeleteRows = false, ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect, MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None, RowHeadersVisible = false, Font = new Font("Microsoft YaHei UI", 10)
            };
            dgvProducts.Columns.Add("Id", "ID");
            dgvProducts.Columns.Add("Name", "\u5546\u54c1\u540d\u79f0");
            dgvProducts.Columns.Add("Price", "\u4ef7\u683c");
            dgvProducts.Columns.Add("Stock", "\u5e93\u5b58");
            dgvProducts.Columns.Add("Category", "\u5206\u7c7b");
            dgvProducts.Columns.Add("CreatedAt", "\u521b\u5efa\u65f6\u95f4");
            dgvProducts.Columns["Id"].FillWeight = 10;
            dgvProducts.Columns["Name"].FillWeight = 35;
            dgvProducts.Columns["Price"].FillWeight = 15;
            dgvProducts.Columns["Stock"].FillWeight = 15;
            dgvProducts.Columns["Category"].FillWeight = 15;
            dgvProducts.Columns["CreatedAt"].FillWeight = 20;

            Controls.Add(dgvProducts);
            Controls.Add(panelTop);
        }

        private void LoadProducts(string searchText = null)
        {
            var products = DatabaseHelper.GetAllProducts(searchText);
            dgvProducts.Rows.Clear();
            foreach (var p in products)
                dgvProducts.Rows.Add(p.Id, p.Name, string.Format("\u00a5{0:F2}", p.Price), p.Stock, p.Category, p.CreatedAt.ToString("yyyy-MM-dd HH:mm"));
        }

        private void BtnSearch_Click(object sender, EventArgs e) { LoadProducts(txtSearch.Text.Trim()); }
        private void BtnAdd_Click(object sender, EventArgs e)
        {
            using (var dialog = new ProductEditDialog(null)) { if (dialog.ShowDialog() == DialogResult.OK) LoadProducts(); }
        }
        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (dgvProducts.SelectedRows.Count == 0) { MessageBox.Show("\u8bf7\u5148\u9009\u62e9\u8981\u7f16\u8f91\u7684\u5546\u54c1", "\u63d0\u793a", MessageBoxButtons.OK, MessageBoxIcon.Information); return; }
            var selectedId = Convert.ToInt32(dgvProducts.SelectedRows[0].Cells["Id"].Value);
            using (var dialog = new ProductEditDialog(selectedId)) { if (dialog.ShowDialog() == DialogResult.OK) LoadProducts(); }
        }
    }

    public class ProductEditDialog : Form
    {
        private TextBox txtName;
        private NumericUpDown nudPrice;
        private NumericUpDown nudStock;
        private ComboBox cmbCategory;
        private readonly int? _productId;

        public ProductEditDialog(int? productId)
        {
            _productId = productId;
            InitializeComponents();
            if (productId.HasValue) LoadProduct(productId.Value);
        }

        private void InitializeComponents()
        {
            Text = _productId.HasValue ? "\u7f16\u8f91\u5546\u54c1" : "\u65b0\u589e\u5546\u54c1";
            Size = new Size(400, 350);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            BackColor = Color.White;

            var lblName = new Label { Text = "\u5546\u54c1\u540d\u79f0:", Font = new Font("Microsoft YaHei UI", 10), Location = new Point(30, 30), AutoSize = true };
            txtName = new TextBox { Font = new Font("Microsoft YaHei UI", 10), Location = new Point(120, 27), Size = new Size(220, 25) };
            var lblPrice = new Label { Text = "\u4ef7   \u683c:", Font = new Font("Microsoft YaHei UI", 10), Location = new Point(30, 70), AutoSize = true };
            nudPrice = new NumericUpDown { Font = new Font("Microsoft YaHei UI", 10), Location = new Point(120, 67), Size = new Size(220, 25), Maximum = 999999, Minimum = 0, DecimalPlaces = 2 };
            var lblStock = new Label { Text = "\u5e93   \u5b58:", Font = new Font("Microsoft YaHei UI", 10), Location = new Point(30, 110), AutoSize = true };
            nudStock = new NumericUpDown { Font = new Font("Microsoft YaHei UI", 10), Location = new Point(120, 107), Size = new Size(220, 25), Maximum = 99999, Minimum = 0 };
            var lblCategory = new Label { Text = "\u5206   \u7c7b:", Font = new Font("Microsoft YaHei UI", 10), Location = new Point(30, 150), AutoSize = true };
            cmbCategory = new ComboBox { Font = new Font("Microsoft YaHei UI", 10), Location = new Point(120, 147), Size = new Size(220, 25), DropDownStyle = ComboBoxStyle.DropDownList };
            cmbCategory.Items.AddRange(new object[] { "\u624b\u673a\u6570\u7801", "\u7535\u8111\u529e\u516c", "\u5e73\u677f\u7535\u8111", "\u667a\u80fd\u7a7f\u6234", "\u97f3\u9891\u8bbe\u5907", "\u6e38\u620f\u5a31\u4e50", "\u7535\u5b50\u9605\u8bfb", "\u5176\u4ed6" });
            cmbCategory.SelectedIndex = 0;

            var btnSave = new Button { Text = "\u4fdd\u5b58", Font = new Font("Microsoft YaHei UI", 10), Size = new Size(100, 35), Location = new Point(120, 210), BackColor = Color.FromArgb(0, 122, 204), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand };
            btnSave.Click += BtnSave_Click;
            var btnCancel = new Button { Text = "\u53d6\u6d88", Font = new Font("Microsoft YaHei UI", 10), Size = new Size(100, 35), Location = new Point(240, 210), BackColor = Color.FromArgb(108, 117, 125), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand };
            btnCancel.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };
            Controls.AddRange(new Control[] { lblName, txtName, lblPrice, nudPrice, lblStock, nudStock, lblCategory, cmbCategory, btnSave, btnCancel });
        }

        private void LoadProduct(int id)
        {
            var product = DatabaseHelper.GetProductById(id);
            if (product != null)
            {
                txtName.Text = product.Name;
                nudPrice.Value = product.Price;
                nudStock.Value = product.Stock;
                var idx = cmbCategory.Items.IndexOf(product.Category);
                if (idx >= 0) cmbCategory.SelectedIndex = idx;
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text)) { MessageBox.Show("\u8bf7\u8f93\u5165\u5546\u54c1\u540d\u79f0", "\u63d0\u793a", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            if (_productId.HasValue)
            {
                var product = DatabaseHelper.GetProductById(_productId.Value);
                if (product != null)
                {
                    product.Name = txtName.Text.Trim();
                    product.Price = nudPrice.Value;
                    product.Stock = (int)nudStock.Value;
                    product.Category = cmbCategory.SelectedItem != null ? cmbCategory.SelectedItem.ToString() : "\u5176\u4ed6";
                    DatabaseHelper.UpdateProduct(product);
                }
            }
            else
            {
                var product = new Product { Name = txtName.Text.Trim(), Price = nudPrice.Value, Stock = (int)nudStock.Value, Category = cmbCategory.SelectedItem != null ? cmbCategory.SelectedItem.ToString() : "\u5176\u4ed6", CreatedAt = DateTime.Now };
                DatabaseHelper.InsertProduct(product);
            }
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}