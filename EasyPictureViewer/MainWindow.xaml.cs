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
        //程序参数
        private string[] args = null;
        //当前图片所在目录
        private string nowDirectory = null;
        //图片文件列表缓存
        private List<string> imageFiles = new List<string>();
        //当前图片在列表中的位置
        private int nowIndex;

        public MainWindow(string[] args)
        {
            InitializeComponent();
            
            //设置窗口图标
            this.Icon = BitmapFrame.Create(new Uri("pack://application:,,,/EasyPictureViewer;component/EasyPictureViewer.ico", UriKind.RelativeOrAbsolute));
            //设置窗口背景
            this.background.Source = BitmapFrame.Create(new Uri("pack://application:,,,/EasyPictureViewer;component/EasyPictureViewerBackground.png", UriKind.RelativeOrAbsolute));

            //保存参数。
            this.args = args;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            int i;

            //初始化状态栏的布局
            statusGrid.Width = statusBarItem.ActualWidth - (statusBarItem.Padding.Left + statusBarItem.Padding.Right);

            if (args.Length == 0)
            {
                //设置打开文件会话
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.CheckPathExists = true;
                openFileDialog.CheckFileExists = true;
                openFileDialog.Filter = SupportImageFiles.GetFilter();
                openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                openFileDialog.Multiselect = false;
                openFileDialog.Title = "EasyPictureViewer: Oepn a file.";
                openFileDialog.ValidateNames = true;

                //显示打开文件对话
                if (openFileDialog.ShowDialog() != true)
                {
                    //如果未打开文件就退出程序
                    Environment.Exit(0);
                }

                //打开用户所选图片文件
                OpenFile(openFileDialog.FileName);
            }
            else
            {
                //打开传输的参数所指图片文件
                OpenFile(args[0]);

                //释放参数
                for (i = 0; i < args.Length; i++)
                {
                    args[i] = null;
                }
                args = null;
            }

            //使image显示
            image.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// 打开文件。
        /// 会刷新文件列表缓存。
        /// </summary>
        /// <param name="path">要打开的图片文件路径。</param>
        public void OpenFile(string path)
        {
            //获取打开的图像文件所在文件夹路径。
            nowDirectory = System.IO.Path.GetDirectoryName(path);
            //设置iamge的图片。
            SetImage(path);
            
            //获取打开的图像所在文件夹下其他的图片文件列表，并缓存{
            //打开文件夹
            DirectoryInfo di = new DirectoryInfo(nowDirectory);
            //获取文件夹下所有文件
            FileInfo[] fis = di.GetFiles();
            //获取图片文件
            FileInfo[] sfis = (from val in fis
                               where Regex.Match(val.Extension, SupportImageFiles.GetExtensionRegularExpression(), RegexOptions.IgnoreCase).Success
                               select val).ToArray();
            //存储文件的名称
            imageFiles.Clear();
            foreach (FileInfo fi in sfis)
            {
                imageFiles.Add(fi.Name);
            }
            //排序这些文件
            imageFiles.Sort();
            //将文件列表加入到filesComboBox
            filesComboBox.Items.Clear();
            foreach (string filename in imageFiles)
            {
                filesComboBox.Items.Add(filename);
            }
            //}

            //获取所打开图像文件的名称
            string thisFile = System.IO.Path.GetFileName(path);
            //记录当前文件在列表中的位置
            nowIndex = GetFileIndexInList(thisFile);
            //设置filesComboBox所选项为当前文件
            needNotRefreshFilesComboBox = true;//不需要对列表的改动进行操作
            filesComboBox.SelectedItem = imageFiles[nowIndex];
        }

        private bool needNotRefreshFilesComboBox = false;
        private void filesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (needNotRefreshFilesComboBox)
            {
                needNotRefreshFilesComboBox = false;
                return;
            }

            //记录所选文件在列表中的位置
            nowIndex = GetFileIndexInList((string)filesComboBox.SelectedItem);

            //获取所选文件的文件名
            string nowFile = imageFiles[nowIndex];

            //设置iamge的图片。
            SetImage(nowFile);
        }

        private int GetFileIndexInList(string name)
        {
            int i;

            //获取目标文件名在列表中的位置
            for (i = 0; i < imageFiles.Count; i++)
            {
                if (imageFiles[i] == name)
                {
                    break;
                }
            }
            if (i >= imageFiles.Count)
            {
                //未找到
                MessageBox.Show("Unknown error: Can't find file in cache list now.", "EasyPictureViewer: Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(-1);
            }
            //返回该文件在列表中的位置
            return i;
        }

        /// <summary>
        /// 设置image的图片。
        /// </summary>
        /// <param name="path">要打开的图片文件路径。</param>
        private void SetImage(string path)
        {
            try
            {
                //加载图片
                image.Source = new BitmapImage(new Uri(Uri.EscapeUriString(System.IO.Path.Combine(nowDirectory, path).Replace('\\', '/')), UriKind.RelativeOrAbsolute));
            }
            catch (Exception ex)
            {
                //加载图片失败
                MessageBox.Show(ex.Message, "EasyPictureViewer: Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(-1);
            }

            //重置image的变换
            imageRotateTransform.Angle = 0;
            scalingTextBox.Text = "100";
            image.Margin = new Thickness(0);

            //更新image的布局
            image.UpdateLayout();
            //重置image的中心
            SetImageCenter(image.ActualWidth / 2, image.ActualHeight / 2, 0);
        }
    }
}
