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

        private void scalingTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            float value = GetScalingValue();

            if (value == -1)
            {
                return;
            }

            ChangeScaling(value);
        }

        private void scalingTextBox_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            float value = GetScalingValue();

            if (value == -1)
            {
                return;
            }

            value += e.Delta / 120 * 10;

            ChangeScaling(value);
        }

        private float GetScalingValue()
        {
            float value = 100;

            try
            {
                value = float.Parse(string.Format("{0:F1}", float.Parse(scalingTextBox.Text)));
            }
            catch (Exception)
            {
                if (scalingTextBox.Text == "")
                {
                    return -1;
                }
                else
                {
                    value = float.Parse(string.Format("{0:F1}", float.Parse(scalingTextBox_TextBefore)));
                }
            }
            return value;
        }

        private void ChangeScaling(float value)
        {
            if (value < 0)
            {
                value = 0;
            }
            else if (value > 30000)
            {
                value = 30000;
            }
            scalingTextBox_TextBefore = String.Format("{0:F0}", value);
            scalingTextBox.Text = scalingTextBox_TextBefore;

            if (value == 0)
            {
                image.Visibility = Visibility.Hidden;
            }
            else
            {
                imageScaleTransform.ScaleX = value / 100;
                imageScaleTransform.ScaleY = value / 100;
                image.Visibility = Visibility.Visible;
            }
        }
    }
}
