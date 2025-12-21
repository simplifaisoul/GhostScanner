using System;
using System.Windows.Forms;

namespace GhostScanner
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// Made by Soul and Lapex
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            
            // Prevent multiple instances
            bool createdNew;
            using (var mutex = new System.Threading.Mutex(true, "GhostScanner_SingleInstance", out createdNew))
            {
                if (!createdNew)
                {
                    MessageBox.Show("Ghost Scanner is already running!", "Ghost Scanner", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                Application.Run(new MainForm());
            }
        }
    }
}