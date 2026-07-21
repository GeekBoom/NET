using System;
using System.Drawing;
using System.Windows.Forms;

namespace ECommerceAssistant.Forms
{
    public class MainForm : Form
    {
        private Panel panelSidebar;
        private Panel panelHeader;
        private Panel panelContent;
        private Label lblHeaderTitle;
        private Label lblUsername;
        private Button btnProducts;
        private Button btnInventory;
        private Button btnOrders;

        private readonly string _username;
        private UserControl _currentView;

        public MainForm(string username)
        {
            _username = username;
            InitializeComponents();
            ShowProducts();
        }

        private void InitializeComponents()
        {
            // 窗体设置
            Text = "电商小助手";
            Size = new Size(1100, 700);
            StartPosition = FormStartPosition.CenterScreen;
            MinimumSize = new Size(900, 600);
            BackColor = Color.FromArgb(240, 240, 240);

            // 顶部栏
            panelHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 50,
                BackColor = Color.FromArgb(0, 122, 204),
                Padding = new Padding(15, 0, 15, 0)
            };

            lblHeaderTitle = new Label
            {
                Text = "电商小助手",
                Font = new Font("Microsoft YaHei UI", 14, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(15, 12)
            };

            lblUsername = new Label
            {
                Text = "当前用户: " + _username,
                Font = new Font("Microsoft YaHei UI", 10),
                ForeColor = Color.White,
                AutoSize = true,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            lblUsername.Location = new Point(panelHeader.Width - lblUsername.Width - 20, 15);
            panelHeader.Resize += (s, e) =>
            {
                lblUsername.Location = new Point(panelHeader.Width - lblUsername.Width - 20, 15);
            };

            panelHeader.Controls.AddRange(new Control[] { lblHeaderTitle, lblUsername });

            // 左侧导航栏
            panelSidebar = new Panel
            {
                Dock = DockStyle.Left,
                Width = 180,
                BackColor = Color.FromArgb(45, 45, 48),
                Padding = new Padding(0, 10, 0, 0)
            };

            btnProducts = CreateNavButton("商品列表", 0);
            btnInventory = CreateNavButton("库存查看", 1);
            btnOrders = CreateNavButton("订单列表", 2);

            btnProducts.Click += (s, e) => ShowProducts();
            btnInventory.Click += (s, e) => ShowInventory();
            btnOrders.Click += (s, e) => ShowOrders();

            panelSidebar.Controls.AddRange(new Control[] { btnProducts, btnInventory, btnOrders });

            // 右侧内容区
            panelContent = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(10)
            };

            // 添加到窗体（顺序重要：先添加Dock.Fill，再添加其他）
            Controls.Add(panelContent);
            Controls.Add(panelSidebar);
            Controls.Add(panelHeader);

            // 默认选中商品列表
            SetActiveNavButton(btnProducts);
        }

        private Button CreateNavButton(string text, int index)
        {
            var button = new Button
            {
                Text = text,
                Font = new Font("Microsoft YaHei UI", 11),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(45, 45, 48),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(180, 45),
                Location = new Point(0, 10 + index * 50),
                Cursor = Cursors.Hand,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(20, 0, 0, 0)
            };
            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseOverBackColor = Color.FromArgb(62, 62, 66);
            return button;
        }

        private void SetActiveNavButton(Button activeButton)
        {
            foreach (Control ctrl in panelSidebar.Controls)
            {
                if (ctrl is Button)
                {
                    var btn = (Button)ctrl;
                    if (btn == activeButton)
                    {
                        btn.BackColor = Color.FromArgb(0, 122, 204);
                    }
                    else
                    {
                        btn.BackColor = Color.FromArgb(45, 45, 48);
                    }
                }
            }
        }

        private void ShowView(UserControl view)
        {
            panelContent.Controls.Clear();
            _currentView = view;
            view.Dock = DockStyle.Fill;
            panelContent.Controls.Add(view);
        }

        private void ShowProducts()
        {
            SetActiveNavButton(btnProducts);
            ShowView(new ProductsUserControl());
        }

        private void ShowInventory()
        {
            SetActiveNavButton(btnInventory);
            ShowView(new InventoryUserControl());
        }

        private void ShowOrders()
        {
            SetActiveNavButton(btnOrders);
            ShowView(new OrdersUserControl());
        }
    }
}
namespace ECommerceAssistant.Forms;

public class MainForm : Form
{
    private Panel panelSidebar = null!;
    private Panel panelHeader = null!;
    private Panel panelContent = null!;
    private Label lblHeaderTitle = null!;
    private Label lblUsername = null!;
    private Button btnProducts = null!;
    private Button btnInventory = null!;
    private Button btnOrders = null!;

    private readonly string _username;
    private UserControl? _currentView;

    public MainForm(string username)
    {
        _username = username;
        InitializeComponents();
        ShowProducts();
    }

    private void InitializeComponents()
    {
        // 窗体设置
        Text = "电商小助手";
        Size = new Size(1100, 700);
        StartPosition = FormStartPosition.CenterScreen;
        MinimumSize = new Size(900, 600);
        BackColor = Color.FromArgb(240, 240, 240);

        // 顶部栏
        panelHeader = new Panel
        {
            Dock = DockStyle.Top,
            Height = 50,
            BackColor = Color.FromArgb(0, 122, 204),
            Padding = new Padding(15, 0, 15, 0)
        };

        lblHeaderTitle = new Label
        {
            Text = "电商小助手",
            Font = new Font("Microsoft YaHei UI", 14, FontStyle.Bold),
            ForeColor = Color.White,
            AutoSize = true,
            Location = new Point(15, 12)
        };

        lblUsername = new Label
        {
            Text = $"当前用户: {_username}",
            Font = new Font("Microsoft YaHei UI", 10),
            ForeColor = Color.White,
            AutoSize = true,
            Anchor = AnchorStyles.Top | AnchorStyles.Right
        };
        lblUsername.Location = new Point(panelHeader.Width - lblUsername.Width - 20, 15);
        panelHeader.Resize += (s, e) =>
        {
            lblUsername.Location = new Point(panelHeader.Width - lblUsername.Width - 20, 15);
        };

        panelHeader.Controls.AddRange(new Control[] { lblHeaderTitle, lblUsername });

        // 左侧导航栏
        panelSidebar = new Panel
        {
            Dock = DockStyle.Left,
            Width = 180,
            BackColor = Color.FromArgb(45, 45, 48),
            Padding = new Padding(0, 10, 0, 0)
        };

        btnProducts = CreateNavButton("商品列表", 0);
        btnInventory = CreateNavButton("库存查看", 1);
        btnOrders = CreateNavButton("订单列表", 2);

        btnProducts.Click += (s, e) => ShowProducts();
        btnInventory.Click += (s, e) => ShowInventory();
        btnOrders.Click += (s, e) => ShowOrders();

        panelSidebar.Controls.AddRange(new Control[] { btnProducts, btnInventory, btnOrders });

        // 右侧内容区
        panelContent = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.White,
            Padding = new Padding(10)
        };

        // 添加到窗体（顺序重要：先添加Dock.Fill，再添加其他）
        Controls.Add(panelContent);
        Controls.Add(panelSidebar);
        Controls.Add(panelHeader);

        // 默认选中商品列表
        SetActiveNavButton(btnProducts);
    }

    private Button CreateNavButton(string text, int index)
    {
        var button = new Button
        {
            Text = text,
            Font = new Font("Microsoft YaHei UI", 11),
            ForeColor = Color.White,
            BackColor = Color.FromArgb(45, 45, 48),
            FlatStyle = FlatStyle.Flat,
            Size = new Size(180, 45),
            Location = new Point(0, 10 + index * 50),
            Cursor = Cursors.Hand,
            TextAlign = ContentAlignment.MiddleLeft,
            Padding = new Padding(20, 0, 0, 0)
        };
        button.FlatAppearance.BorderSize = 0;
        button.FlatAppearance.MouseOverBackColor = Color.FromArgb(62, 62, 66);
        return button;
    }

    private void SetActiveNavButton(Button activeButton)
    {
        foreach (Control ctrl in panelSidebar.Controls)
        {
            if (ctrl is Button btn)
            {
                if (btn == activeButton)
                {
                    btn.BackColor = Color.FromArgb(0, 122, 204);
                }
                else
                {
                    btn.BackColor = Color.FromArgb(45, 45, 48);
                }
            }
        }
    }

    private void ShowView(UserControl view)
    {
        panelContent.Controls.Clear();
        _currentView = view;
        view.Dock = DockStyle.Fill;
        panelContent.Controls.Add(view);
    }

    private void ShowProducts()
    {
        SetActiveNavButton(btnProducts);
        ShowView(new ProductsUserControl());
    }

    private void ShowInventory()
    {
        SetActiveNavButton(btnInventory);
        ShowView(new InventoryUserControl());
    }

    private void ShowOrders()
    {
        SetActiveNavButton(btnOrders);
        ShowView(new OrdersUserControl());
    }
}
