using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ECommerceAssistant.Data;
using ECommerceAssistant.Models;

namespace ECommerceAssistant.Forms
{
    public class InventoryUserControl : UserControl
    {
        private DataGridView dgvInventory;
        private Button btnRefresh;
        private Button btnUpdateStock;
        private Label lblLowStockWarning;

        public InventoryUserControl()
        {
            InitializeComponents();
            LoadInventory();
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

            var lblTitle = new Label
            {
                Text = "库存管理",
                Font = new Font("Microsoft YaHei UI", 12, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(10, 12),
                ForeColor = Color.FromArgb(64, 64, 64)
            };

            btnUpdateStock = new Button
            {
                Text = "更新库存",
                Font = new Font("Microsoft YaHei UI", 9),
                Size = new Size(90, 28),
                Location = new Point(120, 10),
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnUpdateStock.Click += BtnUpdateStock_Click;

            btnRefresh = new Button
            {
                Text = "刷新",
                Font = new Font("Microsoft YaHei UI", 9),
                Size = new Size(70, 28),
                Location = new Point(220, 10),
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnRefresh.Click += (s, e) => LoadInventory();

            panelTop.Controls.AddRange(new Control[] { lblTitle, btnUpdateStock, btnRefresh });

            // 低库存警告标签
            lblLowStockWarning = new Label
            {
                Dock = DockStyle.Top,
                Height = 30,
                BackColor = Color.FromArgb(255, 243, 205),
                ForeColor = Color.FromArgb(133, 106, 2),
                Font = new Font("Microsoft YaHei UI", 9),
                Text = "  提示: 库存低于10的商品以红色标记",
                TextAlign = ContentAlignment.MiddleLeft,
                Visible = false
            };

            // 数据表格
            dgvInventory = new DataGridView
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

            dgvInventory.Columns.Add("Id", "ID");
            dgvInventory.Columns.Add("Name", "商品名称");
            dgvInventory.Columns.Add("Category", "分类");
            dgvInventory.Columns.Add("Stock", "当前库存");
            dgvInventory.Columns.Add("Price", "单价");
            dgvInventory.Columns.Add("StockValue", "库存价值");
            dgvInventory.Columns.Add("Status", "状态");

            dgvInventory.Columns["Id"].FillWeight = 8;
            dgvInventory.Columns["Name"].FillWeight = 30;
            dgvInventory.Columns["Category"].FillWeight = 15;
            dgvInventory.Columns["Stock"].FillWeight = 12;
            dgvInventory.Columns["Price"].FillWeight = 12;
            dgvInventory.Columns["StockValue"].FillWeight = 15;
            dgvInventory.Columns["Status"].FillWeight = 12;

            // 行格式化事件
            dgvInventory.CellFormatting += DgvInventory_CellFormatting;

            // 添加到控件
            Controls.Add(dgvInventory);
            Controls.Add(lblLowStockWarning);
            Controls.Add(panelTop);
        }

        private void LoadInventory()
        {
            var products = DatabaseHelper.GetAllProductsOrderByStock();

            dgvInventory.Rows.Clear();
            int lowStockCount = 0;

            foreach (var p in products)
            {
                var status = p.Stock < 10 ? "低库存" : (p.Stock == 0 ? "缺货" : "正常");
                var stockValue = p.Price * p.Stock;

                dgvInventory.Rows.Add(p.Id, p.Name, p.Category, p.Stock, string.Format("¥{0:F2}", p.Price), string.Format("¥{0:F2}", stockValue), status);

                if (p.Stock < 10)
                    lowStockCount++;
            }

            if (lowStockCount > 0)
            {
                lblLowStockWarning.Text = "  警告: 共有 " + lowStockCount + " 件商品库存不足10，请及时补货！";
                lblLowStockWarning.Visible = true;
            }
            else
            {
                lblLowStockWarning.Visible = false;
            }
        }

        private void DgvInventory_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvInventory.Columns[e.ColumnIndex].Name == "Stock" ||
                dgvInventory.Columns[e.ColumnIndex].Name == "Status")
            {
                if (e.Value is int && (int)e.Value < 10)
                {
                    e.CellStyle.ForeColor = Color.Red;
                    e.CellStyle.Font = new Font(dgvInventory.Font, FontStyle.Bold);
                }
                else if (e.Value is string && ((string)e.Value == "低库存" || (string)e.Value == "缺货"))
                {
                    e.CellStyle.ForeColor = Color.Red;
                    e.CellStyle.Font = new Font(dgvInventory.Font, FontStyle.Bold);
                }
            }
        }

        private void BtnUpdateStock_Click(object sender, EventArgs e)
        {
            if (dgvInventory.SelectedRows.Count == 0)
            {
                MessageBox.Show("请先选择要更新库存的商品", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var selectedId = Convert.ToInt32(dgvInventory.SelectedRows[0].Cells["Id"].Value);
            var currentStock = Convert.ToInt32(dgvInventory.SelectedRows[0].Cells["Stock"].Value);

            var product = DatabaseHelper.GetProductById(selectedId);
            if (product == null) return;

            var inputForm = new Form
            {
                Text = "更新库存",
                Size = new Size(350, 180),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                BackColor = Color.White
            };
            var lblInput = new Label { Text = "请输入 [" + product.Name + "] 的新库存:", Font = new Font("Microsoft YaHei UI", 10), Location = new Point(20, 20), AutoSize = true };
            var txtInput = new TextBox { Font = new Font("Microsoft YaHei UI", 10), Location = new Point(20, 50), Size = new Size(280, 25), Text = currentStock.ToString() };
            var btnOk = new Button { Text = "确定", Font = new Font("Microsoft YaHei UI", 9), Size = new Size(80, 30), Location = new Point(120, 90), DialogResult = DialogResult.OK };
            var btnCancel = new Button { Text = "取消", Font = new Font("Microsoft YaHei UI", 9), Size = new Size(80, 30), Location = new Point(210, 90), DialogResult = DialogResult.Cancel };
            inputForm.Controls.AddRange(new Control[] { lblInput, txtInput, btnOk, btnCancel });
            inputForm.AcceptButton = btnOk;
            inputForm.CancelButton = btnCancel;

            if (inputForm.ShowDialog() == DialogResult.OK)
            {
                int newStock;
                if (int.TryParse(txtInput.Text, out newStock) && newStock >= 0)
                {
                    DatabaseHelper.UpdateProductStock(selectedId, newStock);
                    MessageBox.Show("库存更新成功！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadInventory();
                }
                else
                {
                    MessageBox.Show("请输入有效的数字", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
