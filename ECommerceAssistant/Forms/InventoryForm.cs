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

        public InventoryUserControl() { InitializeComponents(); LoadInventory(); }

        private void InitializeComponents()
        {
            var panelTop = new Panel { Dock = DockStyle.Top, Height = 50, BackColor = Color.FromArgb(250, 250, 250), Padding = new Padding(10) };
            var lblTitle = new Label { Text = "\u5e93\u5b58\u7ba1\u7406", Font = new Font("Microsoft YaHei UI", 12, FontStyle.Bold), AutoSize = true, Location = new Point(10, 12), ForeColor = Color.FromArgb(64, 64, 64) };
            btnUpdateStock = new Button { Text = "\u66f4\u65b0\u5e93\u5b58", Font = new Font("Microsoft YaHei UI", 9), Size = new Size(90, 28), Location = new Point(120, 10), BackColor = Color.FromArgb(0, 122, 204), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand };
            btnUpdateStock.Click += BtnUpdateStock_Click;
            btnRefresh = new Button { Text = "\u5237\u65b0", Font = new Font("Microsoft YaHei UI", 9), Size = new Size(70, 28), Location = new Point(220, 10), BackColor = Color.FromArgb(108, 117, 125), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand };
            btnRefresh.Click += (s, e) => LoadInventory();
            panelTop.Controls.AddRange(new Control[] { lblTitle, btnUpdateStock, btnRefresh });

            lblLowStockWarning = new Label { Dock = DockStyle.Top, Height = 30, BackColor = Color.FromArgb(255, 243, 205), ForeColor = Color.FromArgb(133, 106, 2), Font = new Font("Microsoft YaHei UI", 9), Text = "  \u63d0\u793a: \u5e93\u5b58\u4f4e\u4e8e10\u7684\u5546\u54c1\u5c06\u4ee5\u7ea2\u8272\u6807\u8bb0", TextAlign = ContentAlignment.MiddleLeft, Visible = false };

            dgvInventory = new DataGridView { Dock = DockStyle.Fill, AllowUserToAddRows = false, AllowUserToDeleteRows = false, ReadOnly = true, SelectionMode = DataGridViewSelectionMode.FullRowSelect, MultiSelect = false, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, BackgroundColor = Color.White, BorderStyle = BorderStyle.None, RowHeadersVisible = false, Font = new Font("Microsoft YaHei UI", 10) };
            dgvInventory.Columns.Add("Id", "ID");
            dgvInventory.Columns.Add("Name", "\u5546\u54c1\u540d\u79f0");
            dgvInventory.Columns.Add("Category", "\u5206\u7c7b");
            dgvInventory.Columns.Add("Stock", "\u5f53\u524d\u5e93\u5b58");
            dgvInventory.Columns.Add("Price", "\u5355\u4ef7");
            dgvInventory.Columns.Add("StockValue", "\u5e93\u5b58\u4ef7\u503c");
            dgvInventory.Columns.Add("Status", "\u72b6\u6001");
            dgvInventory.Columns["Id"].FillWeight = 8;
            dgvInventory.Columns["Name"].FillWeight = 30;
            dgvInventory.Columns["Category"].FillWeight = 15;
            dgvInventory.Columns["Stock"].FillWeight = 12;
            dgvInventory.Columns["Price"].FillWeight = 12;
            dgvInventory.Columns["StockValue"].FillWeight = 15;
            dgvInventory.Columns["Status"].FillWeight = 12;
            dgvInventory.CellFormatting += DgvInventory_CellFormatting;
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
                var status = p.Stock < 10 ? "\u4f4e\u5e93\u5b58" : (p.Stock == 0 ? "\u7f3a\u8d27" : "\u6b63\u5e38");
                dgvInventory.Rows.Add(p.Id, p.Name, p.Category, p.Stock, string.Format("\u00a5{0:F2}", p.Price), string.Format("\u00a5{0:F2}", p.Price * p.Stock), status);
                if (p.Stock < 10) lowStockCount++;
            }
            if (lowStockCount > 0) { lblLowStockWarning.Text = "  \u8b66\u544a: \u5171\u6709 " + lowStockCount + " \u4ef6\u5546\u54c1\u5e93\u5b58\u4e0d\u8db310\uff0c\u8bf7\u53ca\u65f6\u8865\u8d27\uff01"; lblLowStockWarning.Visible = true; }
            else { lblLowStockWarning.Visible = false; }
        }

        private void DgvInventory_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvInventory.Columns[e.ColumnIndex].Name == "Stock" || dgvInventory.Columns[e.ColumnIndex].Name == "Status")
            {
                if (e.Value is int && (int)e.Value < 10) { e.CellStyle.ForeColor = Color.Red; e.CellStyle.Font = new Font(dgvInventory.Font, FontStyle.Bold); }
                else if (e.Value is string && ((string)e.Value == "\u4f4e\u5e93\u5b58" || (string)e.Value == "\u7f3a\u8d27")) { e.CellStyle.ForeColor = Color.Red; e.CellStyle.Font = new Font(dgvInventory.Font, FontStyle.Bold); }
            }
        }

        private void BtnUpdateStock_Click(object sender, EventArgs e)
        {
            if (dgvInventory.SelectedRows.Count == 0) { MessageBox.Show("\u8bf7\u5148\u9009\u62e9\u8981\u66f4\u65b0\u5e93\u5b58\u7684\u5546\u54c1", "\u63d0\u793a", MessageBoxButtons.OK, MessageBoxIcon.Information); return; }
            var selectedId = Convert.ToInt32(dgvInventory.SelectedRows[0].Cells["Id"].Value);
            var currentStock = Convert.ToInt32(dgvInventory.SelectedRows[0].Cells["Stock"].Value);
            var product = DatabaseHelper.GetProductById(selectedId);
            if (product == null) return;
            var inputForm = new Form { Text = "\u66f4\u65b0\u5e93\u5b58", Size = new Size(350, 180), StartPosition = FormStartPosition.CenterParent, FormBorderStyle = FormBorderStyle.FixedDialog, MaximizeBox = false, BackColor = Color.White };
            var lblInput = new Label { Text = "\u8bf7\u8f93\u5165 [" + product.Name + "] \u7684\u65b0\u5e93\u5b58:", Font = new Font("Microsoft YaHei UI", 10), Location = new Point(20, 20), AutoSize = true };
            var txtInput = new TextBox { Font = new Font("Microsoft YaHei UI", 10), Location = new Point(20, 50), Size = new Size(280, 25), Text = currentStock.ToString() };
            var btnOk = new Button { Text = "\u786e\u5b9a", Font = new Font("Microsoft YaHei UI", 9), Size = new Size(80, 30), Location = new Point(120, 90), DialogResult = DialogResult.OK };
            var btnCancel = new Button { Text = "\u53d6\u6d88", Font = new Font("Microsoft YaHei UI", 9), Size = new Size(80, 30), Location = new Point(210, 90), DialogResult = DialogResult.Cancel };
            inputForm.Controls.AddRange(new Control[] { lblInput, txtInput, btnOk, btnCancel });
            inputForm.AcceptButton = btnOk;
            inputForm.CancelButton = btnCancel;
            if (inputForm.ShowDialog() == DialogResult.OK)
            {
                int newStock;
                if (int.TryParse(txtInput.Text, out newStock) && newStock >= 0)
                { DatabaseHelper.UpdateProductStock(selectedId, newStock); MessageBox.Show("\u5e93\u5b58\u66f4\u65b0\u6210\u529f\uff01", "\u6210\u529f", MessageBoxButtons.OK, MessageBoxIcon.Information); LoadInventory(); }
                else { MessageBox.Show("\u8bf7\u8f93\u5165\u6709\u6548\u7684\u6570\u5b57", "\u9519\u8bef", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            }
        }
    }
}