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
        private void canvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            
        }

        public void RecalculateImageLayout()
        {
            double x = 0, y = 0;

            //计算image实际显示水平和垂直长度
            double iaw, iah;
            if (imageRotateTransform.Angle % 180 == 0)
            {
                //0度或180度旋转
                iaw = image.ActualWidth;
                iah = image.ActualHeight;
            }
            else
            {
                //90度或270度旋转
                iah = image.ActualWidth;
                iaw = image.ActualHeight;
            }

            //计算画布中心
            double cwm = canvas.ActualWidth / 2;
            double chm = canvas.ActualHeight / 2;

            imageRotateTransform.CenterX = cwm;
            imageRotateTransform.CenterY = chm;
            imageScaleTransform.CenterX = cwm;
            imageScaleTransform.CenterY = chm;

            //计算正常图片放置到中心

        }
    }
}
