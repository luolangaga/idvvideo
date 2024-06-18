using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
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
using UiDesktopApp2.ViewModels.Pages;

namespace UiDesktopApp2.Views.Pages
{
    /// <summary>
    /// build_floder.xaml 的交互逻辑
    /// </summary>
    public partial class build_floder : Page
    {
        string _model;
        public build_floder(string model)
        {
            InitializeComponent();
            _model = model;
            if (model == "录像")
            {
text.Text = "新建录像包";
            }
            else if (model == "录像夹")
            {
                text.Text = "新建录像夹";
            }

        }

        static void CopyDirectories(string sourcePath, string targetPath)
        {
            // 遍历源路径下的所有子目录和文件
            foreach (var item in Directory.GetFileSystemEntries(sourcePath))
            {
                string itemName = Path.GetFileName(item);
                string sourceItemPath = item;
                string targetItemPath = Path.Combine(targetPath, itemName);

                // 如果是文件，则直接复制到对应的目标文件夹下
                if (File.Exists(sourceItemPath))
                {
                    // 确保目标文件夹存在
                    if (!Directory.Exists(targetPath))
                    {
                        Directory.CreateDirectory(targetPath);
                    }
                    File.Copy(sourceItemPath, Path.Combine(targetItemPath), true);
                    Console.WriteLine($"文件 {itemName} 已复制.");
                }
                // 如果是目录，则递归处理
                else if (Directory.Exists(sourceItemPath))
                {
                    // 创建对应的目标子目录
                    Directory.CreateDirectory(targetItemPath);
                    // 递归调用，处理子目录
                    CopyDirectories(sourceItemPath, targetItemPath);
                }
            }
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
        private static void AddDirectoryToZipArchive(ZipArchive zipArchive, string sourceDirectory, string directoryNameInZip)
        {
            var directoryInfo = new DirectoryInfo(sourceDirectory);

            foreach (var fileInfo in directoryInfo.GetFiles())
            {
                zipArchive.CreateEntryFromFile(fileInfo.FullName, Path.Combine(directoryNameInZip, fileInfo.Name));
            }

            foreach (var subDirectoryInfo in directoryInfo.GetDirectories())
            {
                // 递归添加子目录及其内容
                AddDirectoryToZipArchive(zipArchive, subDirectoryInfo.FullName, Path.Combine(directoryNameInZip, subDirectoryInfo.Name));
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (_model == "录像")
            {
                var foldersToCompress = DashboardViewModel._colors.Where(a => a.ischeck == true).Select(a => a.path).ToList();
                // 压缩后ZIP文件的路径
                string packFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "pack");

                // 确保pack文件夹存在
                if (!Directory.Exists(packFolderPath))
                {
                    Directory.CreateDirectory(packFolderPath);
                }

             
                try
                {
                    // 生成随机的GUID作为文件名
                    string guidFileName = $"{flodername.Text}.idvpack";
                    // 压缩后ZIP文件的完整路径
                    string zipFilePath = Path.Combine(packFolderPath, guidFileName);

                    using (var zipArchive = ZipFile.Open(zipFilePath, ZipArchiveMode.Create))
                    {
                        foreach (var sourceDirectory in foldersToCompress)
                        {
                            // 获取目录名称作为在ZIP中的条目路径
                            var directoryNameInZip = Path.GetFileName(sourceDirectory);

                            // 遍历源目录下的所有文件和子目录，逐个添加到ZIP中
                            AddDirectoryToZipArchive(zipArchive, sourceDirectory, directoryNameInZip);
                        }
                    }


                    var messageBox = new Wpf.Ui.Controls.MessageBox
                    {

                        Title = "成功！",
                        Content = new TextBlock
                        {
                            Text = $"打包成功！",
                            TextWrapping = TextWrapping.Wrap,
                        },
                        PrimaryButtonText = "我知道了",


                        ShowInTaskbar = false,
                        Topmost = false,
                        ResizeMode = ResizeMode.NoResize,
                    };
                    //注册关闭事件 用于打开文件夹
                    messageBox.Closed += async (sender, args) =>
                    {
                        await Task.Delay(500);
                        System.Diagnostics.Process.Start("explorer.exe", packFolderPath);
                    };
                    messageBox.ShowDialogAsync();
                    //TODO:打开文件夹


                }
                catch (Exception ex)
                {
                    Console.WriteLine("压缩过程中发生错误: " + ex.Message);
                    Showinf("失败", $"打包失败！\r\n原因：{ex.Message}", "确定");


                }
            }
            else if (_model == "录像夹")
            {
                try
                {
                    var foldersToCompress = DashboardViewModel._colors.Where(a => a.ischeck == true).Select(a => a.path).ToList();
                    string mubiaoFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "vfolder");
                    if (!Directory.Exists(Path.Combine(mubiaoFolder, flodername.Text)))
                    {
                        Directory.CreateDirectory(Path.Combine(mubiaoFolder, flodername.Text));
                        foreach (var folder in foldersToCompress)
                        {
                            var video_name = new DirectoryInfo(folder).Name;
                            if (!Directory.Exists(Path.Combine(mubiaoFolder, flodername.Text)))
                            {
                                Directory.CreateDirectory(Path.Combine(mubiaoFolder, flodername.Text, video_name));

                            }

                            // 复制文件夹的所有内容到目标文件夹
                            CopyDirectories(folder, Path.Combine(mubiaoFolder, flodername.Text, video_name));

                            Showinf(Title: "成功", cont: "新建成功", button_text: "确定");

                        }
                    }
                    else
                    {
                        Showinf(Title: "错误", cont: "同名录像夹已存在", button_text: "确定");
                    }


                }
                catch (Exception)
                {

                    throw;
                }
            }
           
        }
    }
}
