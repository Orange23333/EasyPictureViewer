using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace EasyPictureViewer
{
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            Application app = new Application();
            MainWindow mainWindow = new MainWindow(args);
            app.MainWindow = mainWindow;
            mainWindow.Show();
            app.Run();
        }
    }
}
