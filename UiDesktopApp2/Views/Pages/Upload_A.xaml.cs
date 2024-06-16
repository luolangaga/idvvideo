using AdvancedSharpAdbClient.Models;
using AdvancedSharpAdbClient;
using System;
using System.Collections.Generic;
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
using AdvancedSharpAdbClient.DeviceCommands.Models;
using AdvancedSharpAdbClient.DeviceCommands;
using AdvancedSharpAdbClient.Receivers;
using System.IO;
using System.Diagnostics;
using UiDesktopApp2.Models;
using UiDesktopApp2.ViewModels.Pages;

namespace UiDesktopApp2.Views.Pages
{
    /// <summary>
    /// Upload_A.xaml 的交互逻辑
    /// </summary>
   
public partial class Upload_A : Page
    {
        List<_server> servers = new List<_server>(new[] {
            new _server { Nmae = "网易官服", PackName = "com.netease.dwrg" },
            new _server { Nmae = "VIVO服", PackName = "com.netease.dwrg5.vivo" },
            new _server {PackName = "com.netease.idv.googleplay",Nmae = "亚服"},
            new _server {PackName = "com.netease.dwrg.mi",Nmae = "米服"}
        });
        class _server
        {
            public string Nmae { get; set; }
            public string PackName { get; set; }

        }
        
        /// <summary>
        /// 同步执行adb命令
        /// </summary>
        /// <param name="arguments">命令参数(adb除外)</param>
        /// <param name="ouputFunc">命令执行正确返回</param>
        /// <param name="errOuputFunc">命令执行错误返回</param>
        /// <returns>执行返回的正确返回和错误返回 格式：正确返回\r\n错误返回</returns>
        public string RunADB(string arguments)
        {
            string cmd = $@"{AppDomain.CurrentDomain.BaseDirectory}Adb\adb.exe";
            Process p = new Process();
            p.StartInfo.FileName = cmd;           //设定程序名  
            p.StartInfo.Arguments = arguments;    //设定程式执行參數  
            p.StartInfo.UseShellExecute = false;        //关闭Shell的使用  
            p.StartInfo.RedirectStandardInput = true;   //重定向标准输入  
            p.StartInfo.RedirectStandardOutput = true;  //重定向标准输出  
            p.StartInfo.RedirectStandardError = true;   //重定向错误输出  
            p.StartInfo.StandardInputEncoding = Encoding.UTF8; // 指定输入流编码为 UTF-8
            p.StartInfo.CreateNoWindow = true;          //设置不显示窗口  
            p.StartInfo.StandardOutputEncoding = Encoding.UTF8; // 指定输出流编码为 UTF-8
            p.StartInfo.StandardErrorEncoding = Encoding.UTF8; // 指定错误输出流编码为 UTF-8
            p.Start();
            string result = p.StandardOutput.ReadToEnd(); // 正确输出
          //  var errOuput = p.StandardError.ReadToEnd(); // 错误输出
           // p.Close();

        
            return result;
        }

        public static async Task<string> RunADB1(string arguments)
        {
            string cmd = $@"{AppDomain.CurrentDomain.BaseDirectory}Adb\adb.exe";
            Process p = new Process();
            p.StartInfo.FileName = cmd;           //设定程序名  
            p.StartInfo.Arguments = arguments;    //设定程式执行參數  
            p.StartInfo.UseShellExecute = false;        //关闭Shell的使用  
            p.StartInfo.RedirectStandardInput = true;   //重定向标准输入  
            p.StartInfo.RedirectStandardOutput = true;  //重定向标准输出  
            p.StartInfo.RedirectStandardError = true;   //重定向错误输出  
            p.StartInfo.CreateNoWindow = true;  
            //设置不显示窗口  
            p.StartInfo.StandardInputEncoding = Encoding.UTF8; // 指定输入流编码为 UTF-8
            p.StartInfo.StandardOutputEncoding = Encoding.UTF8; // 指定输出流编码为 UTF-8
            p.StartInfo.StandardErrorEncoding = Encoding.UTF8; // 指定错误输出流编码为 UTF-8
            p.Start();
            await p.WaitForExitAsync();
            string result = p.StandardOutput.ReadToEnd(); // 正确输出
                                                          //  var errOuput = p.StandardError.ReadToEnd(); // 错误输出
                                                          // p.Close();


            return result;
        }


        public  Upload_A()
        {
            //     DataContext = this;
          
            

            InitializeComponent();
            idv_server.ItemsSource = servers.Select(a => a.Nmae);
            client_DeviceConnected();
            //创建一个AdbServer对象

        }
        AdbClient client;
        DeviceData device;
        DeviceClient deviceClient;
        private async void client_DeviceConnected()
        {
            Task.Run(() =>
            {
                      RunADB("devices");
            });
            await Task.Delay(2000);
           
            client = new AdbClient();
              
                var devices = client.GetDevices();
            allphone.Items.Clear();
            foreach (var device in devices)
                {
                    allphone.Items.Add(device.Name);
                }

   

            Console.WriteLine("");
          //  MessageBox.Show("设备已连接");
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
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
          
            panel1.Visibility = Visibility.Hidden;
            panel2.Visibility = Visibility.Visible;
            device = client.GetDevices().FirstOrDefault(a => a.Name == allphone.SelectedItem.ToString());
            // client.ConnectAsync()
            //获取设备列表
        }

     

        private void idv_server_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        public List<(string Name, DateTime LastWriteTime)> GetFoldersWithLastWriteTime(string directoryPath)
        {
            var directoryInfo = new DirectoryInfo(directoryPath);
            var folders = directoryInfo.GetDirectories();

            var folderList = new List<(string Name, DateTime LastWriteTime)>();

            foreach (var folder in folders)
            {
                folderList.Add((folder.FullName, folder.LastWriteTime));
            }

            return folderList;
        }
        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
             try
             {
                progress.Visibility = Visibility.Visible;
                string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "vfolder");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);

                }
                if(string.IsNullOrEmpty(idv_id.Text))
                {
                    Showinf(Title = "错误", cont: "请输入第五人格ID", button_text: "确定");
                    return;
                }
                string output = await RunADB1($"pull -a /sdcard/Android/data/{servers.FirstOrDefault(a=>a.Nmae==idv_server.SelectedValue.ToString()).PackName}/files/netease/dwrg.common/Documents/video/{idv_id.Text} {path}");
               
                Showinf(Title = "成功！", cont: $"已导入录像", button_text: "确定");

                progress.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
             {
               Showinf(Title = "错误", cont: ex.Message, button_text: "确定");

            }
        }
    }
}
