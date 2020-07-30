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
        private double scaling = 1;
        //private string scalingTextBox_TextBefore = "100";//Need to delete

        private void scalingTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            double value = GetScalingValue();

            if (value == -1)
            {
                return;
            }

            ChangeScaling(value);
        }

        private void scalingTextBox_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            double value = GetScalingValue();

            if (value == -1)
            {
                return;
            }

            value += e.Delta / 120 * 10;

            ChangeScaling(value);
        }

        private double GetScalingValue()
        {
            double value = 100;

            try
            {
                value = double.Parse(string.Format("{0:F1}", double.Parse(scalingTextBox.Text)));
            }
            catch (Exception)
            {
                if (scalingTextBox.Text == "")
                {
                    return -1;
                }
                else
                {
                    value = double.Parse(string.Format("{0:F1}", scaling*100));
                }
            }
            return value;
        }

        private void ChangeScaling(double value)
        {
            if (value < 0)
            {
                value = 0;
            }
            else if (value > 10000)
            {
                value = 10000;
            }
            scaling = value / 100;
            scalingTextBox.Text = String.Format("{0:F1}", scaling * 100);

            if (scaling == 0)
            {
                image.Visibility = Visibility.Hidden;
            }
            else
            {
                imageScaleTransform.ScaleX = scaling;
                imageScaleTransform.ScaleY = scaling;
                image.Visibility = Visibility.Visible;
            }
        }
    }
}
