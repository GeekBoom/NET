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
            // 顶部工具栏
            var panelTop = new Panel
            {
                Dock = DockStyle.Top,
                Height = 50,
                BackColor = Color.FromArgb(250, 250, 250),
                Padding = new Padding(10, 10, 10, 10)
            };

            var lblSearch = new Label
            {
                Text = "搜索:",
                Font = new Font("Microsoft YaHei UI", 10),
                AutoSize = true,
                Location = new Point(10, 14)
            };

            txtSearch = new TextBox
            {
                Font = new Font("Microsoft YaHei UI", 10),
                Location = new Point(55, 11),
                Size = new Size(200, 25)
            };

            btnSearch = new Button
            {
                Text = "搜索",
                Font = new Font("Microsoft YaHei UI", 9),
                Size = new Size(70, 28),
                Location = new Point(265, 10),
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnSearch.Click += BtnSearch_Click;

            btnAdd = new Button
            {
                Text = "新增商品",
                Font = new Font("Microsoft YaHei UI", 9),
                Size = new Size(90, 28),
                Location = new Point(350, 10),
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnAdd.Click += BtnAdd_Click;

            btnEdit = new Button
            {
                Text = "编辑商品",
                Font = new Font("Microsoft YaHei UI", 9),
                Size = new Size(90, 28),
                Location = new Point(450, 10),
                BackColor = Color.FromArgb(255, 193, 7),
                ForeColor = Color.Black,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnEdit.Click += BtnEdit_Click;

            btnRefresh = new Button
            {
                Text = "刷新",
                Font = new Font("Microsoft YaHei UI", 9),
                Size = new Size(70, 28),
                Location = new Point(550, 10),
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnRefresh.Click += (s, e) => LoadProducts();

            panelTop.Controls.AddRange(new Control[] { lblSearch, txtSearch, btnSearch, btnAdd, btnEdit, btnRefresh });

            // 数据表格
            dgvProducts = new DataGridView
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

            dgvProducts.Columns.Add("Id", "ID");
            dgvProducts.Columns.Add("Name", "商品名称");
            dgvProducts.Columns.Add("Price", "价格");
            dgvProducts.Columns.Add("Stock", "库存");
            dgvProducts.Columns.Add("Category", "分类");
            dgvProducts.Columns.Add("CreatedAt", "创建时间");

            dgvProducts.Columns["Id"].FillWeight = 10;
            dgvProducts.Columns["Name"].FillWeight = 35;
            dgvProducts.Columns["Price"].FillWeight = 15;
            dgvProducts.Columns["Stock"].FillWeight = 15;
            dgvProducts.Columns["Category"].FillWeight = 15;
            dgvProducts.Columns["CreatedAt"].FillWeight = 20;

            // 添加到控件
            Controls.Add(dgvProducts);
            Controls.Add(panelTop);
        }

        private void LoadProducts(string searchText = null)
        {
            var products = DatabaseHelper.GetAllProducts(searchText);

            dgvProducts.Rows.Clear();
            foreach (var p in products)
            {
                dgvProducts.Rows.Add(p.Id, p.Name, string.Format("¥{0:F2}", p.Price), p.Stock, p.Category, p.CreatedAt.ToString("yyyy-MM-dd HH:mm"));
            }
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            LoadProducts(txtSearch.Text.Trim());
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            using (var dialog = new ProductEditDialog(null))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    LoadProducts();
                }
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (dgvProducts.SelectedRows.Count == 0)
            {
                MessageBox.Show("请先选择要编辑的商品", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var selectedId = Convert.ToInt32(dgvProducts.SelectedRows[0].Cells["Id"].Value);
            using (var dialog = new ProductEditDialog(selectedId))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    LoadProducts();
                }
            }
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
            if (productId.HasValue)
            {
                LoadProduct(productId.Value);
            }
        }

        private void InitializeComponents()
        {
            Text = _productId.HasValue ? "编辑商品" : "新增商品";
            Size = new Size(400, 350);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            BackColor = Color.White;

            var lblName = new Label { Text = "商品名称:", Font = new Font("Microsoft YaHei UI", 10), Location = new Point(30, 30), AutoSize = true };
            txtName = new TextBox { Font = new Font("Microsoft YaHei UI", 10), Location = new Point(120, 27), Size = new Size(220, 25) };

            var lblPrice = new Label { Text = "价    格:", Font = new Font("Microsoft YaHei UI", 10), Location = new Point(30, 70), AutoSize = true };
            nudPrice = new NumericUpDown
            {
                Font = new Font("Microsoft YaHei UI", 10),
                Location = new Point(120, 67),
                Size = new Size(220, 25),
                Maximum = 999999,
                Minimum = 0,
                DecimalPlaces = 2,
                Value = 0
            };

            var lblStock = new Label { Text = "库    存:", Font = new Font("Microsoft YaHei UI", 10), Location = new Point(30, 110), AutoSize = true };
            nudStock = new NumericUpDown
            {
                Font = new Font("Microsoft YaHei UI", 10),
                Location = new Point(120, 107),
                Size = new Size(220, 25),
                Maximum = 99999,
                Minimum = 0,
                Value = 0
            };

            var lblCategory = new Label { Text = "分    类:", Font = new Font("Microsoft YaHei UI", 10), Location = new Point(30, 150), AutoSize = true };
            cmbCategory = new ComboBox
            {
                Font = new Font("Microsoft YaHei UI", 10),
                Location = new Point(120, 147),
                Size = new Size(220, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbCategory.Items.AddRange(new object[] { "手机数码", "电脑办公", "平板电脑", "智能穿戴", "音频设备", "游戏娱乐", "电子阅读", "其他" });
            cmbCategory.SelectedIndex = 0;

            var btnSave = new Button
            {
                Text = "保存",
                Font = new Font("Microsoft YaHei UI", 10),
                Size = new Size(100, 35),
                Location = new Point(120, 210),
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnSave.Click += BtnSave_Click;

            var btnCancel = new Button
            {
                Text = "取消",
                Font = new Font("Microsoft YaHei UI", 10),
                Size = new Size(100, 35),
                Location = new Point(240, 210),
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
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
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("请输入商品名称", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_productId.HasValue)
            {
                var product = DatabaseHelper.GetProductById(_productId.Value);
                if (product != null)
                {
                    product.Name = txtName.Text.Trim();
                    product.Price = nudPrice.Value;
                    product.Stock = (int)nudStock.Value;
                    product.Category = cmbCategory.SelectedItem != null ? cmbCategory.SelectedItem.ToString() : "其他";
                    DatabaseHelper.UpdateProduct(product);
                }
            }
            else
            {
                var product = new Product
                {
                    Name = txtName.Text.Trim(),
                    Price = nudPrice.Value,
                    Stock = (int)nudStock.Value,
                    Category = cmbCategory.SelectedItem != null ? cmbCategory.SelectedItem.ToString() : "其他",
                    CreatedAt = DateTime.Now
                };
                DatabaseHelper.InsertProduct(product);
            }

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
using ECommerceAssistant.Data;
using ECommerceAssistant.Models;

namespace ECommerceAssistant.Forms;

public class ProductsUserControl : UserControl
{
    private DataGridView dgvProducts = null!;
    private TextBox txtSearch = null!;
    private Button btnAdd = null!;
    private Button btnEdit = null!;
    private Button btnRefresh = null!;
    private Button btnSearch = null!;

    public ProductsUserControl()
    {
        InitializeComponents();
        LoadProducts();
    }

    private void InitializeComponents()
    {
        // 顶部工具栏
        var panelTop = new Panel
        {
            Dock = DockStyle.Top,
            Height = 50,
            BackColor = Color.FromArgb(250, 250, 250),
            Padding = new Padding(10, 10, 10, 10)
        };

        var lblSearch = new Label
        {
            Text = "搜索:",
            Font = new Font("Microsoft YaHei UI", 10),
            AutoSize = true,
            Location = new Point(10, 14)
        };

        txtSearch = new TextBox
        {
            Font = new Font("Microsoft YaHei UI", 10),
            Location = new Point(55, 11),
            Size = new Size(200, 25)
        };

        btnSearch = new Button
        {
            Text = "搜索",
            Font = new Font("Microsoft YaHei UI", 9),
            Size = new Size(70, 28),
            Location = new Point(265, 10),
            BackColor = Color.FromArgb(0, 122, 204),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand
        };
        btnSearch.Click += BtnSearch_Click;

        btnAdd = new Button
        {
            Text = "新增商品",
            Font = new Font("Microsoft YaHei UI", 9),
            Size = new Size(90, 28),
            Location = new Point(350, 10),
            BackColor = Color.FromArgb(40, 167, 69),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand
        };
        btnAdd.Click += BtnAdd_Click;

        btnEdit = new Button
        {
            Text = "编辑商品",
            Font = new Font("Microsoft YaHei UI", 9),
            Size = new Size(90, 28),
            Location = new Point(450, 10),
            BackColor = Color.FromArgb(255, 193, 7),
            ForeColor = Color.Black,
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand
        };
        btnEdit.Click += BtnEdit_Click;

        btnRefresh = new Button
        {
            Text = "刷新",
            Font = new Font("Microsoft YaHei UI", 9),
            Size = new Size(70, 28),
            Location = new Point(550, 10),
            BackColor = Color.FromArgb(108, 117, 125),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand
        };
        btnRefresh.Click += (s, e) => LoadProducts();

        panelTop.Controls.AddRange(new Control[] { lblSearch, txtSearch, btnSearch, btnAdd, btnEdit, btnRefresh });

        // 数据表格
        dgvProducts = new DataGridView
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

        dgvProducts.Columns.Add("Id", "ID");
        dgvProducts.Columns.Add("Name", "商品名称");
        dgvProducts.Columns.Add("Price", "价格");
        dgvProducts.Columns.Add("Stock", "库存");
        dgvProducts.Columns.Add("Category", "分类");
        dgvProducts.Columns.Add("CreatedAt", "创建时间");

        dgvProducts.Columns["Id"]!.FillWeight = 10;
        dgvProducts.Columns["Name"]!.FillWeight = 35;
        dgvProducts.Columns["Price"]!.FillWeight = 15;
        dgvProducts.Columns["Stock"]!.FillWeight = 15;
        dgvProducts.Columns["Category"]!.FillWeight = 15;
        dgvProducts.Columns["CreatedAt"]!.FillWeight = 20;

        // 添加到控件
        Controls.Add(dgvProducts);
        Controls.Add(panelTop);
    }

    private void LoadProducts(string? searchText = null)
    {
        using var context = new AppDbContext();
        var query = context.Products.AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchText))
        {
            query = query.Where(p =>
                p.Name.Contains(searchText) ||
                p.Category.Contains(searchText));
        }

        var products = query.OrderByDescending(p => p.CreatedAt).ToList();

        dgvProducts.Rows.Clear();
        foreach (var p in products)
        {
            dgvProducts.Rows.Add(p.Id, p.Name, $"¥{p.Price:F2}", p.Stock, p.Category, p.CreatedAt.ToString("yyyy-MM-dd HH:mm"));
        }
    }

    private void BtnSearch_Click(object? sender, EventArgs e)
    {
        LoadProducts(txtSearch.Text.Trim());
    }

    private void BtnAdd_Click(object? sender, EventArgs e)
    {
        using var dialog = new ProductEditDialog(null);
        if (dialog.ShowDialog() == DialogResult.OK)
        {
            LoadProducts();
        }
    }

    private void BtnEdit_Click(object? sender, EventArgs e)
    {
        if (dgvProducts.SelectedRows.Count == 0)
        {
            MessageBox.Show("请先选择要编辑的商品", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var selectedId = (int)dgvProducts.SelectedRows[0].Cells["Id"].Value;
        using var dialog = new ProductEditDialog(selectedId);
        if (dialog.ShowDialog() == DialogResult.OK)
        {
            LoadProducts();
        }
    }
}

public class ProductEditDialog : Form
{
    private TextBox txtName = null!;
    private NumericUpDown nudPrice = null!;
    private NumericUpDown nudStock = null!;
    private ComboBox cmbCategory = null!;
    private readonly int? _productId;

    public ProductEditDialog(int? productId)
    {
        _productId = productId;
        InitializeComponents();
        if (productId.HasValue)
        {
            LoadProduct(productId.Value);
        }
    }

    private void InitializeComponents()
    {
        Text = _productId.HasValue ? "编辑商品" : "新增商品";
        Size = new Size(400, 350);
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        BackColor = Color.White;

        var lblName = new Label { Text = "商品名称:", Font = new Font("Microsoft YaHei UI", 10), Location = new Point(30, 30), AutoSize = true };
        txtName = new TextBox { Font = new Font("Microsoft YaHei UI", 10), Location = new Point(120, 27), Size = new Size(220, 25) };

        var lblPrice = new Label { Text = "价    格:", Font = new Font("Microsoft YaHei UI", 10), Location = new Point(30, 70), AutoSize = true };
        nudPrice = new NumericUpDown
        {
            Font = new Font("Microsoft YaHei UI", 10),
            Location = new Point(120, 67),
            Size = new Size(220, 25),
            Maximum = 999999,
            Minimum = 0,
            DecimalPlaces = 2,
            Value = 0
        };

        var lblStock = new Label { Text = "库    存:", Font = new Font("Microsoft YaHei UI", 10), Location = new Point(30, 110), AutoSize = true };
        nudStock = new NumericUpDown
        {
            Font = new Font("Microsoft YaHei UI", 10),
            Location = new Point(120, 107),
            Size = new Size(220, 25),
            Maximum = 99999,
            Minimum = 0,
            Value = 0
        };

        var lblCategory = new Label { Text = "分    类:", Font = new Font("Microsoft YaHei UI", 10), Location = new Point(30, 150), AutoSize = true };
        cmbCategory = new ComboBox
        {
            Font = new Font("Microsoft YaHei UI", 10),
            Location = new Point(120, 147),
            Size = new Size(220, 25),
            DropDownStyle = ComboBoxStyle.DropDownList
        };
        cmbCategory.Items.AddRange(new object[] { "手机数码", "电脑办公", "平板电脑", "智能穿戴", "音频设备", "游戏娱乐", "电子阅读", "其他" });
        cmbCategory.SelectedIndex = 0;

        var btnSave = new Button
        {
            Text = "保存",
            Font = new Font("Microsoft YaHei UI", 10),
            Size = new Size(100, 35),
            Location = new Point(120, 210),
            BackColor = Color.FromArgb(0, 122, 204),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand
        };
        btnSave.Click += BtnSave_Click;

        var btnCancel = new Button
        {
            Text = "取消",
            Font = new Font("Microsoft YaHei UI", 10),
            Size = new Size(100, 35),
            Location = new Point(240, 210),
            BackColor = Color.FromArgb(108, 117, 125),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand
        };
        btnCancel.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };

        Controls.AddRange(new Control[] { lblName, txtName, lblPrice, nudPrice, lblStock, nudStock, lblCategory, cmbCategory, btnSave, btnCancel });
    }

    private void LoadProduct(int id)
    {
        using var context = new AppDbContext();
        var product = context.Products.Find(id);
        if (product != null)
        {
            txtName.Text = product.Name;
            nudPrice.Value = product.Price;
            nudStock.Value = product.Stock;
            var idx = cmbCategory.Items.IndexOf(product.Category);
            if (idx >= 0) cmbCategory.SelectedIndex = idx;
        }
    }

    private void BtnSave_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtName.Text))
        {
            MessageBox.Show("请输入商品名称", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        using var context = new AppDbContext();

        if (_productId.HasValue)
        {
            var product = context.Products.Find(_productId.Value);
            if (product != null)
            {
                product.Name = txtName.Text.Trim();
                product.Price = nudPrice.Value;
                product.Stock = (int)nudStock.Value;
                product.Category = cmbCategory.SelectedItem?.ToString() ?? "其他";
                context.SaveChanges();
            }
        }
        else
        {
            context.Products.Add(new Product
            {
                Name = txtName.Text.Trim(),
                Price = nudPrice.Value,
                Stock = (int)nudStock.Value,
                Category = cmbCategory.SelectedItem?.ToString() ?? "其他",
                CreatedAt = DateTime.Now
            });
            context.SaveChanges();
        }

        DialogResult = DialogResult.OK;
        Close();
    }
}
