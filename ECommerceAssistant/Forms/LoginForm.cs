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
            // 窗体设置
            Text = "电商小助手 - 登录";
            Size = new Size(400, 350);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            BackColor = Color.White;

            // 标题
            lblTitle = new Label
            {
                Text = "电商小助手",
                Font = new Font("Microsoft YaHei UI", 18, FontStyle.Bold),
                ForeColor = Color.FromArgb(64, 64, 64),
                AutoSize = true,
                Location = new Point(130, 30)
            };

            // 用户名标签
            lblUsername = new Label
            {
                Text = "用户名:",
                Font = new Font("Microsoft YaHei UI", 10),
                Location = new Point(60, 100),
                AutoSize = true
            };

            // 用户名输入框
            txtUsername = new TextBox
            {
                Font = new Font("Microsoft YaHei UI", 10),
                Location = new Point(140, 97),
                Size = new Size(180, 28),
                Text = "admin"
            };

            // 密码标签
            lblPassword = new Label
            {
                Text = "密  码:",
                Font = new Font("Microsoft YaHei UI", 10),
                Location = new Point(60, 145),
                AutoSize = true
            };

            // 密码输入框
            txtPassword = new TextBox
            {
                Font = new Font("Microsoft YaHei UI", 10),
                Location = new Point(140, 142),
                Size = new Size(180, 28),
                UseSystemPasswordChar = true,
                Text = "123456"
            };

            // 登录按钮
            btnLogin = new Button
            {
                Text = "登  录",
                Font = new Font("Microsoft YaHei UI", 11, FontStyle.Bold),
                Size = new Size(180, 40),
                Location = new Point(140, 200),
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnLogin.Click += BtnLogin_Click;

            // 错误提示
            lblError = new Label
            {
                Text = "",
                Font = new Font("Microsoft YaHei UI", 9),
                ForeColor = Color.Red,
                Location = new Point(60, 255),
                Size = new Size(280, 30)
            };

            // 添加到窗体
            Controls.AddRange(new Control[]
            {
                lblTitle, lblUsername, txtUsername,
                lblPassword, txtPassword, btnLogin, lblError
            });

            // 回车键触发登录
            AcceptButton = btnLogin;
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            var username = txtUsername.Text.Trim();
            var password = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                lblError.Text = "请输入用户名和密码";
                return;
            }

            var user = DatabaseHelper.FindUser(username, password);

            if (user == null)
            {
                lblError.Text = "用户名或密码错误";
                return;
            }

            LoggedInUsername = user.Username;
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
using ECommerceAssistant.Data;
using ECommerceAssistant.Models;

namespace ECommerceAssistant.Forms;

public class LoginForm : Form
{
    private Label lblTitle = null!;
    private Label lblUsername = null!;
    private Label lblPassword = null!;
    private TextBox txtUsername = null!;
    private TextBox txtPassword = null!;
    private Button btnLogin = null!;
    private Label lblError = null!;

    public string? LoggedInUsername { get; private set; }

    public LoginForm()
    {
        InitializeComponents();
    }

    private void InitializeComponents()
    {
        // 窗体设置
        Text = "电商小助手 - 登录";
        Size = new Size(400, 350);
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        BackColor = Color.White;

        // 标题
        lblTitle = new Label
        {
            Text = "电商小助手",
            Font = new Font("Microsoft YaHei UI", 18, FontStyle.Bold),
            ForeColor = Color.FromArgb(64, 64, 64),
            AutoSize = true,
            Location = new Point(130, 30)
        };

        // 用户名标签
        lblUsername = new Label
        {
            Text = "用户名:",
            Font = new Font("Microsoft YaHei UI", 10),
            Location = new Point(60, 100),
            AutoSize = true
        };

        // 用户名输入框
        txtUsername = new TextBox
        {
            Font = new Font("Microsoft YaHei UI", 10),
            Location = new Point(140, 97),
            Size = new Size(180, 28),
            Text = "admin"
        };

        // 密码标签
        lblPassword = new Label
        {
            Text = "密  码:",
            Font = new Font("Microsoft YaHei UI", 10),
            Location = new Point(60, 145),
            AutoSize = true
        };

        // 密码输入框
        txtPassword = new TextBox
        {
            Font = new Font("Microsoft YaHei UI", 10),
            Location = new Point(140, 142),
            Size = new Size(180, 28),
            UseSystemPasswordChar = true,
            Text = "123456"
        };

        // 登录按钮
        btnLogin = new Button
        {
            Text = "登  录",
            Font = new Font("Microsoft YaHei UI", 11, FontStyle.Bold),
            Size = new Size(180, 40),
            Location = new Point(140, 200),
            BackColor = Color.FromArgb(0, 122, 204),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand
        };
        btnLogin.Click += BtnLogin_Click;

        // 错误提示
        lblError = new Label
        {
            Text = "",
            Font = new Font("Microsoft YaHei UI", 9),
            ForeColor = Color.Red,
            Location = new Point(60, 255),
            Size = new Size(280, 30)
        };

        // 添加到窗体
        Controls.AddRange(new Control[]
        {
            lblTitle, lblUsername, txtUsername,
            lblPassword, txtPassword, btnLogin, lblError
        });

        // 回车键触发登录
        AcceptButton = btnLogin;
    }

    private void BtnLogin_Click(object? sender, EventArgs e)
    {
        var username = txtUsername.Text.Trim();
        var password = txtPassword.Text.Trim();

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            lblError.Text = "请输入用户名和密码";
            return;
        }

        using var context = new AppDbContext();
        var user = context.Users.FirstOrDefault(u =>
            u.Username == username && u.Password == password);

        if (user == null)
        {
            lblError.Text = "用户名或密码错误";
            return;
        }

        LoggedInUsername = user.Username;
        DialogResult = DialogResult.OK;
        Close();
    }
}
