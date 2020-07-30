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
        private double xOffset = 0;
        private double yOffset = 0;

        private void SetImageCenter(double xOff, double yOff, double angle)
        {
            imageRotateTransform.CenterX = xOff;
            imageRotateTransform.CenterY = yOff;
            imageScaleTransform.CenterX = 0;
            imageScaleTransform.CenterY = 0;
        }

        private void canvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double x = 0, y = 0;
            //计算画布中心
            double cwm = canvas.ActualWidth / 2;
            double chm = canvas.ActualHeight / 2;
            SetImageCenter(cwm, chm, imageRotateTransform.Angle);

            //计算正常图片放置到中心
            x += xOffset;
            y += yOffset;
        }
    }
}
