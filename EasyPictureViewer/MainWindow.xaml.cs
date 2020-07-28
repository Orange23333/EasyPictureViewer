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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string[] args = null;
        private string nowDirectory = null;
        private List<string> imageFiles = new List<string>();
        private int nowIndex;

        public MainWindow(string[] args)
        {
            InitializeComponent();
            
            this.Icon = BitmapFrame.Create(new Uri("pack://application:,,,/EasyPictureViewer;component/EasyPictureViewer.ico", UriKind.RelativeOrAbsolute));
            this.background.Source = BitmapFrame.Create(new Uri("pack://application:,,,/EasyPictureViewer;component/EasyPictureViewerBackground.png", UriKind.RelativeOrAbsolute));

            this.args = args;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //int i;
            //string arg;
            //StringBuilder sb = new StringBuilder();
            //for(i = 0; i < args.Length; i++)
            //{
            //    arg = args[i];
            //    sb.Append(String.Format("[{0}]:\"{1}\"\n", i, arg));
            //}
            //MessageBox.Show(sb.ToString());

            statusGrid.Width = statusBarItem.ActualWidth - (statusBarItem.Padding.Left + statusBarItem.Padding.Right);

            scalingTextBox_TextBefore = scalingTextBox.Text;

            if (args.Length == 0)
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                
                openFileDialog.CheckPathExists = true;
                openFileDialog.CheckFileExists = true;
                openFileDialog.Filter = SupportImageFiles.GetFilter();
                openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                openFileDialog.Multiselect = false;
                openFileDialog.Title = "EasyPictureViewer: Oepn a file.";
                openFileDialog.ValidateNames = true;

                if (openFileDialog.ShowDialog() != true)
                {
                    Environment.Exit(0);
                }

                OpenFile(openFileDialog.FileName);
            }
            else
            {
                OpenFile(args[0]);
            }

            image.Visibility = Visibility.Visible;
        }

        public void OpenFile(string path)
        {
            int i;

            nowDirectory = System.IO.Path.GetDirectoryName(path);
            SetImage(path);
            
            DirectoryInfo di = new DirectoryInfo(nowDirectory);
            FileInfo[] fis = di.GetFiles();
            FileInfo[] sfis = (from val in fis
                               where Regex.Match(val.Extension, SupportImageFiles.GetExtensionRegularExpression(), RegexOptions.IgnoreCase).Success
                               select val).ToArray();
            imageFiles.Clear();
            filesComboBox.Items.Clear();
            foreach (FileInfo fi in sfis)
            {
                imageFiles.Add(fi.Name);
            }
            imageFiles.Sort();
            foreach (string filename in imageFiles)
            {
                filesComboBox.Items.Add(filename);
            }

            string thisFile = System.IO.Path.GetFileName(path);
            for (i = 0; i < imageFiles.Count; i++)
            {
                if (imageFiles[i] == thisFile)
                {
                    break;
                }
            }
            if (i >= imageFiles.Count)
            {
                MessageBox.Show("Unknow error: Can't find file now.", "EasyPictureViewer: Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(-1);
            }
            nowIndex = i;
            filesComboBox.SelectedItem = imageFiles[nowIndex];
        }

        private void filesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int i;

            for (i = 0; i < imageFiles.Count; i++)
            {
                if (imageFiles[i] == (string)filesComboBox.SelectedItem)
                {
                    break;
                }
            }
            if (i >= imageFiles.Count)
            {
                MessageBox.Show("Unknow error: Can't find file now.", "EasyPictureViewer: Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(-1);
            }
            nowIndex = i;

            string nowFile = imageFiles[nowIndex];

            SetImage(nowFile);
            filesComboBox.SelectedItem = nowFile;
        }

        private void SetImage(string path)
        {
            try
            {
                image.Source = new BitmapImage(new Uri(Uri.EscapeUriString(System.IO.Path.Combine(nowDirectory, path).Replace('\\', '/')), UriKind.RelativeOrAbsolute));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "EasyPictureViewer: Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(-1);
            }

            imageRotateTransform.Angle = 0;
            imageScaleTransform.ScaleX = 0;
            imageScaleTransform.ScaleY = 0;
            image.Margin = new Thickness(0);

            image.UpdateLayout();
            SetImageCenter(image.ActualWidth / 2, image.ActualHeight / 2);
        }
    }
}
