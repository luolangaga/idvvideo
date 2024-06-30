using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Wpf.Ui;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;
using System.Threading;
using System.Diagnostics;
using Flurl.Http;
using Path = System.IO.Path;
using System.IO.Compression;

namespace UiDesktopApp2.Views.Windows
{


    /// <summary>
    /// Download.xaml 的交互逻辑
    /// </summary>
    public partial class Download :FluentWindow
    {

      


        List<string> Urls;
        public Download(List<string> _urls,string _savepath,string _zippath)
        {
            InitializeComponent();
            Urls = _urls;
            Savepath = _savepath;
            Zippath = _zippath;
            StartDownload();
        }


        private HttpClient client = new HttpClient();
         private long totalBytes;
        private string url = "12"; // 目标文档URL，请替换为实际地址
        private string Savepath ; // 目标文档URL，请替换为实际地址
        private string Zippath; // 目标文档URL，请替换为实际地址
        private DateTime lastUpdate;


        public async void StartDownload()
        {
            try
            {
                int a = 0;

                foreach (var item in Urls)
                {
                    url = item;
                    var uuidN = Guid.NewGuid().ToString("N");
                    // 生成随机的GUID作为文件名
                    string guidFileName = $"{uuidN}.idvpack";

                    //   await item.DownloadFileAsync(packFolderPath, guidFileName);
                     string pa = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "pack", guidFileName);
                  //  string pa = Path.Combine(Savepath);
                    string zipFilePath = pa;
                    Random ran = new Random();
                    string n = ran.Next(9999999).ToString();
                    //  string destinationFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "vfolder", $"share{n}"); // 解压的目标目录
                    string destinationFolder =Zippath; // 解压的目标目录

                    string localPath = zipFilePath; // 替换为你想保存的路径
                     

                    // 确保目标目录存在，如果需要的话创建它



                   
                      try
                    {

                        using (var response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                totalBytes = response.Content.Headers.ContentLength.Value;
                                processp.Maximum = 100;
                                await DownloadFileAsync(response,zipFilePath);
                            }
                            else
                            {
                                Showinf("失败",$"下载失败，状态码：{response.StatusCode}","确定");
                            }
                        }

                        await Task.Delay(500);

                        ZipFile.ExtractToDirectory(zipFilePath, destinationFolder);
                        a++;
                    }
                    catch (OperationCanceledException)
                    {
                        Showinf("用户取消","下载已取消。","我知道了");
                    }
                    catch (Exception ex)
                    {
                        Showinf("下载失败", $"下载失败的{ex.Message}。", "我知道了");

                    }
                }
                Showinf("成功", $"共下载{a}个文件", "确定");

            }
            catch (Exception)
            {

                throw;
            }



        }


        private async Task DownloadFileAsync(HttpResponseMessage response,string file_path)
        {
            var fileName = url.Split('/').Last(); // 获取文件名
            var fileStream = await response.Content.ReadAsStreamAsync();
            download_mt.Text = $"正在下载：{fileName}";
            using (var file = System.IO.File.Create(file_path))
            {
                var buffer = new byte[4096];
                int bytesRead;
                long totalRead = 0;
                lastUpdate = DateTime.UtcNow;

                while ((bytesRead = await fileStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    await file.WriteAsync(buffer, 0, bytesRead);
                    totalRead += bytesRead;

                    // 计算并更新进度条与速度显示
                    UpdateProgress(totalRead);

                    // 每秒更新一次速度显示
                    if (DateTime.UtcNow - lastUpdate >= TimeSpan.FromSeconds(1))
                    {
                        spendtext.Text = $"已下载: {CalculateSpeed(totalRead).ToString("0.00")} MB";
                     
                        lastUpdate = DateTime.UtcNow;
                    }
                }
            }
        }

        private void UpdateProgress(long totalRead)
        {
            var progress = (double)totalRead / totalBytes * 100;
            processp.Value = ((int)progress);
            procresstext.Text = $"进度: {progress:F2}%";
        }

        private Double CalculateSpeed(long totalRead)
        {
            var timeSpan = DateTime.UtcNow - lastUpdate;
            return (totalRead / timeSpan.TotalSeconds/ 1048576);
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
      

    }
}
