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
            SeedData.Initialize();
            using (var loginForm = new LoginForm())
            {
                if (loginForm.ShowDialog() != DialogResult.OK) return;
                var mainForm = new MainForm(loginForm.LoggedInUsername ?? "Unknown");
                Application.Run(mainForm);
            }
        }
    }
}