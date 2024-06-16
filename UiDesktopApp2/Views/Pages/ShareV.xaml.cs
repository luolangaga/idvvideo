using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

using System.Windows.Shapes;
using Path = System.IO.Path;
using Flurl.Http;
using UiDesktopApp2.ViewModels.Pages;
using UiDesktopApp2.Models;

namespace UiDesktopApp2.Views.Pages
{
    /// <summary>
    /// ShareV.xaml 的交互逻辑
    /// </summary>
    public partial class ShareV : Page
    {
        public ShareV()
        {
            InitializeComponent();
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
            process.Visibility = Visibility.Visible;

            try { 
            var foldersToCompress = DataViewModel._Allvideo.Where(a => a.ischeck == true).Select(a => a._path).ToList();
            var url_names = new List<string>();
            foreach (var folder in foldersToCompress)
            {
                var uuidN = Guid.NewGuid().ToString("N");
                // 生成随机的GUID作为文件名
                string guidFileName = $"{uuidN}.idvpack";
                string packFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "pack");

                // 压缩后ZIP文件的完整路径
                string zipFilePath = Path.Combine(packFolderPath, guidFileName);

                ZipFile.CreateFromDirectory(folder, zipFilePath);



                var a = await $"{Config.http_url}/api/v1/updata_video".PostMultipartAsync(mp => mp.AddFile("imageFile", zipFilePath));
                var reqtext = await a.GetStringAsync();
                url_names.Add($"{Config.http_url}/video/{reqtext}");
            }
            Clipboard.SetDataObject($"{JsonSerializer.Serialize(url_names)}\r\n|请复制后使用“第五人格录像管理器”打开，即可自动导入,你的好友也可以在社区找到这个录像。");
             await $"{Config.http_url}/api/v1/videos".PostJsonAsync(new Video { Video_name=flodername.Text, Video_msg=flodermsg.Text, Video_url= JsonSerializer.Serialize(url_names) });
          
                Showinf("分享成功！", $"请将粘贴出的文本发送给好友，好友复制后打开软件即可自动导入！{string.Join(",", url_names)}", "确定");
          
        }
            catch (Exception ex)
            {
                Showinf("失败", $"失败！\r\n原因：{ex.Message}", "确定");

    }
            process.Visibility=Visibility.Hidden;
}
    }
    public class Video
    {
         public string Video_name { get; set; }
        public string Video_msg { get; set; }
        public string Video_url { get; set; }

    }
}
