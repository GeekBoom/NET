using System;
using System.Drawing;
using System.Windows.Forms;
using ECommerceAssistant.Data;

namespace ECommerceAssistant.Forms
{
    public class LoginForm : Form
    {
        private Label lblTitle;
        private Label lblUsername;
        private Label lblPassword;
        private TextBox txtUsername;
        private TextBox txtPassword;
        private Button btnLogin;
        private Label lblError;

        public string LoggedInUsername { get; private set; }

        public LoginForm()
        {
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            Text = "\u7535\u5546\u5c0f\u52a9\u624b - \u767b\u5f55";
            Size = new Size(400, 350);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            BackColor = Color.White;

            lblTitle = new Label { Text = "\u7535\u5546\u5c0f\u52a9\u624b", Font = new Font("Microsoft YaHei UI", 18, FontStyle.Bold), ForeColor = Color.FromArgb(64, 64, 64), AutoSize = true, Location = new Point(130, 30) };
            lblUsername = new Label { Text = "\u7528\u6237\u540d:", Font = new Font("Microsoft YaHei UI", 10), Location = new Point(60, 100), AutoSize = true };
            txtUsername = new TextBox { Font = new Font("Microsoft YaHei UI", 10), Location = new Point(140, 97), Size = new Size(180, 28), Text = "admin" };
            lblPassword = new Label { Text = "\u5bc6  \u7801:", Font = new Font("Microsoft YaHei UI", 10), Location = new Point(60, 145), AutoSize = true };
            txtPassword = new TextBox { Font = new Font("Microsoft YaHei UI", 10), Location = new Point(140, 142), Size = new Size(180, 28), UseSystemPasswordChar = true, Text = "123456" };
            btnLogin = new Button { Text = "\u767b  \u5f55", Font = new Font("Microsoft YaHei UI", 11, FontStyle.Bold), Size = new Size(180, 40), Location = new Point(140, 200), BackColor = Color.FromArgb(0, 122, 204), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand };
            btnLogin.Click += BtnLogin_Click;
            lblError = new Label { Text = "", Font = new Font("Microsoft YaHei UI", 9), ForeColor = Color.Red, Location = new Point(60, 255), Size = new Size(280, 30) };

            Controls.AddRange(new Control[] { lblTitle, lblUsername, txtUsername, lblPassword, txtPassword, btnLogin, lblError });
            AcceptButton = btnLogin;
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            var username = txtUsername.Text.Trim();
            var password = txtPassword.Text.Trim();
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password)) { lblError.Text = "\u8bf7\u8f93\u5165\u7528\u6237\u540d\u548c\u5bc6\u7801"; return; }
            var user = DatabaseHelper.FindUser(username, password);
            if (user == null) { lblError.Text = "\u7528\u6237\u540d\u6216\u5bc6\u7801\u9519\u8bef"; return; }
            LoggedInUsername = user.Username;
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}