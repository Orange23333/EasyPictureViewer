using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace EasyPictureViewer
{
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            //MessageBox.Show(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);

            //bool needExit = false;
            //if (args.Contains("--reg-icon"))
            //{
            //    //注册图标
            //    ;
            //    
            //}
            //if (args.Contains("--reg-icon-force"))
            //{
            //    //强制注册图标
            //    ;
            //}
            //if (args.Contains("--reg-boot"))
            //{
            //    //关联程序
            //    ;
            //}
            //if (args.Contains("--reg-icon-force"))
            //{
            //    //强制管理程序
            //    ;
            //}
            //if (needExit)
            //{
            //    Environment.Exit(0);
            //}

            Application app = new Application();
            MainWindow mainWindow = new MainWindow(args);
            app.MainWindow = mainWindow;
            mainWindow.Show();
            app.Run();
        }
    }
}
