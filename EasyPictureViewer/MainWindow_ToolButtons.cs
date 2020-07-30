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
        private int moveSourceIndex = -1;

        private void leftButton_Click(object sender, RoutedEventArgs e)
        {
            if (--nowIndex < 0)
            {
                nowIndex = imageFiles.Count - 1;
            }

            string prevFile = imageFiles[nowIndex];

            if((new FileInfo(System.IO.Path.Combine(nowDirectory, prevFile).Replace('\\', '/'))).Exists)
            {
                moveSourceIndex = -1;

                SetImage(prevFile);
                needNotRefreshFilesComboBox = true;
                filesComboBox.SelectedItem = prevFile;
            }
            else
            {
                if (moveSourceIndex != -1)
                {
                    moveSourceIndex = nowIndex;
                }
                else if(moveSourceIndex == nowIndex)
                {
                    MessageBox.Show("Error: No image file in this directory.", "EasyPictureViewer: Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    Environment.Exit(-1);
                }
                leftButton_Click(sender, e);
            }
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

            if ((new FileInfo(System.IO.Path.Combine(nowDirectory, nextFile).Replace('\\', '/'))).Exists)
            {
                moveSourceIndex = -1;

                SetImage(nextFile);
                needNotRefreshFilesComboBox = true;
                filesComboBox.SelectedItem = nextFile;
            }
            else
            {
                if (moveSourceIndex != -1)
                {
                    moveSourceIndex = nowIndex;
                }
                else if (moveSourceIndex == nowIndex)
                {
                    MessageBox.Show("Error: No image file in this directory.", "EasyPictureViewer: Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    Environment.Exit(-1);
                }
                rightButton_Click(sender, e);
            }
        }
    }
}
