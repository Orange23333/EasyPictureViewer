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
        private double rotationAngle = 0;

        private void contrarotateButton_Click(object sender, RoutedEventArgs e)
        {
            imageRotateTransform.Angle -= 90;
            if (imageRotateTransform.Angle < 0)
            {
                imageRotateTransform.Angle = 270;
            }
        }

        private void clockwiseRotationButton_Click(object sender, RoutedEventArgs e)
        {
            imageRotateTransform.Angle += 90;
            if (imageRotateTransform.Angle >= 360)
            {
                imageRotateTransform.Angle = 0;
            }
        }
    }
}
