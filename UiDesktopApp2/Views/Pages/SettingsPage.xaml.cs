using System.Diagnostics;
using System.IO;
using UiDesktopApp2.ViewModels.Pages;
using Wpf.Ui.Controls;

namespace UiDesktopApp2.Views.Pages
{
    public partial class SettingsPage : INavigableView<SettingsViewModel>
    {
        public SettingsViewModel ViewModel { get; }

        public SettingsPage(SettingsViewModel viewModel,bool showabout=true)
        {
            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();
            if (!showabout)
            {
                about.Visibility = Visibility.Hidden;
                about_text.Visibility = Visibility.Hidden;
                clear_button.Visibility = Visibility.Hidden;
            }
            else
            {
                try
                {
                    long sizebybyte = GetDirectorySize(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "pack"));
                    clear_button.Content = $"清理({sizebybyte / (1024 * 1024)}MB)";

                }
                catch (Exception)
                {

                   
                }
                  }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
           
            Process.Start("explorer.exe", "https://b23.tv/syI3Z4R");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Process.Start("explorer.exe", "https://dotnet.microsoft.com/zh-cn/");
           
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Process.Start("explorer.exe", "https://visualstudio.microsoft.com/zh-hans/");

        }
        static long GetDirectorySize(string path)
        {
            long totalSize = 0;

            // 获取文件夹中的所有文件和子文件夹
            string[] files = Directory.GetFiles(path);
            string[] subDirs = Directory.GetDirectories(path);

            // 计算所有文件的大小
            foreach (string file in files)
            {
                FileInfo fileInfo = new FileInfo(file);
                totalSize += fileInfo.Length;
            }

            // 递归计算所有子文件夹的大小，并累加到总大小中
            foreach (string subDir in subDirs)
            {
                totalSize += GetDirectorySize(subDir);
            }

            return totalSize;
        }
        private void Showinf(string Title, object cont, string button_text)
        {
            var messageBox = new Wpf.Ui.Controls.MessageBox
            {

                Title = Title,
                Content = cont

                ,
                PrimaryButtonText = button_text,


                ShowInTaskbar = false,
                Topmost = false,
                ResizeMode = ResizeMode.NoResize,
            };

            messageBox.ShowDialogAsync();
        }
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            MIT mIT = new MIT();
            Showinf(Title: "关于", cont:mIT , button_text: "确定");
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            Process.Start("explorer.exe", "https://wiki.biligame.com/dwrg/");

        }
        static void DeleteAllFilesInDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Console.WriteLine($"路径 '{path}' 不存在。");
                return;
            }

            // 获取目录中的所有文件
            string[] files = Directory.GetFiles(path);

            foreach (string file in files)
            {
                File.Delete(file);
            }
        }
        private void clear_button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DeleteAllFilesInDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "pack"));
                Showinf("成功", "清除成功", "我知道了");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"发生错误: {ex.Message}");
            }
   
        }
    }
}
