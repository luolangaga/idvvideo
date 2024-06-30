using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
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
using UiDesktopApp2.Views.Windows;
using Path = System.IO.Path;

namespace UiDesktopApp2.Views.Pages
{
    /// <summary>
    /// idvlogin.xaml 的交互逻辑
    /// </summary>
    public partial class idvlogin : Page
    {
        public idvlogin()
        {
            InitializeComponent();
            shuax();
        }
        private void shuax()
        {
            if (!Path.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "idvlogin", "idvlogin.exe")))
            {
                No_downloadpanel.Visibility = Visibility.Visible;
                Have_downloadpanel.Visibility = Visibility.Hidden;
            }
        }
        private  void Button_Click(object sender, RoutedEventArgs e)
        {
            string packFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "idvlogin");

            // 确保pack文件夹存在
            if (!Directory.Exists(packFolderPath))
            {
                Directory.CreateDirectory(packFolderPath);
            }
            var a=new List<string>();
            a.Add("https://gitee.com/luolan1/idvvideo/releases/download/idvlogin/idvlogin.zip");
            Download download = new Download(a, packFolderPath, packFolderPath);
            download.Show();
            download.Closed += Download_Closed;
           // await Task.Delay(2000);
          
        }

        private void Download_Closed(object? sender, EventArgs e)
        {
            shuax();
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ShowMsg.Text = "启动成功！你现在可以打开游戏了";
            successpic.Visibility= Visibility.Visible;
            start_but.Visibility = Visibility.Hidden;

            ProcessStartInfo start = new ProcessStartInfo("idvlogin\\idvlogin.exe");
         //   start.Arguments = "/C ping www.google.com";
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;
            start.CreateNoWindow = true;
            using (Process process = new Process())
            {
                process.StartInfo = start;
                process.Start();
                  process.BeginOutputReadLine();
                await process.WaitForExitAsync();
            }
           
        }

    }
}
