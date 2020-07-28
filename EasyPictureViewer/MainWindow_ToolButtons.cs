using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EasyPictureViewer
{
    public partial class MainWindow : Window
    {
        private void leftButton_Click(object sender, RoutedEventArgs e)
        {
            if (--nowIndex < 0)
            {
                nowIndex = imageFiles.Count - 1;
            }

            string prevFile = imageFiles[nowIndex];

            SetImage(prevFile);
            filesComboBox.SelectedItem = prevFile;
        }

        private void rightButton_Click(object sender, RoutedEventArgs e)
        {
            if (nowIndex == int.MaxValue)
            {
                nowIndex = 0;
            }
            else
            {
                if (++nowIndex >= imageFiles.Count)
                {
                    nowIndex = 0;
                }
            }

            string nextFile = imageFiles[nowIndex];

            SetImage(nextFile);
            filesComboBox.SelectedItem = nextFile;
        }
    }
}
