using System;
using System.Windows.Forms;
using ECommerceAssistant.Forms;

namespace ECommerceAssistant
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // 初始化数据库和种子数据
            SeedData.Initialize();

            // 显示登录窗口
            using (var loginForm = new LoginForm())
            {
                if (loginForm.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                // 登录成功，显示主界面
                var mainForm = new MainForm(loginForm.LoggedInUsername ?? "Unknown");
                Application.Run(mainForm);
            }
        }
    }
}
using ECommerceAssistant.Forms;

namespace ECommerceAssistant;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();

        // 初始化数据库和种子数据
        SeedData.Initialize();

        // 显示登录窗口
        using var loginForm = new LoginForm();
        if (loginForm.ShowDialog() != DialogResult.OK)
        {
            return;
        }

        // 登录成功，显示主界面
        var mainForm = new MainForm(loginForm.LoggedInUsername ?? "Unknown");
        Application.Run(mainForm);
    }
}
