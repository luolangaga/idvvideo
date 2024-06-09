using Microsoft.Win32;
using System.IO;
using System.IO.Compression;
using System.Security.Policy;
using System.Windows.Media;
using UiDesktopApp2.Models;
using UiDesktopApp2.Views.Pages;
using Wpf.Ui.Controls;
using static System.Net.Mime.MediaTypeNames;
using MessageBoxResult = System.Windows.MessageBoxResult;

namespace UiDesktopApp2.ViewModels.Pages
{
    public partial class DataViewModel : ObservableObject
    {
        
        public DataViewModel()
        {
            InitializeViewModel();
        }
        private bool _isInitialized = false;

        [ObservableProperty]
        private IEnumerable<DataVideo> _Allvideo;
        [ObservableProperty]
        private string _allcheck_text="全选";


        [RelayCommand]
        private void OnRestart()
        {
            InitializeViewModel();
        }


        /// <summary>
        /// 复制文件夹及文件
        /// </summary>
        /// <param name="sourceFolder">原文件路径</param>
        /// <param name="destFolder">目标文件路径</param>
        /// <returns></returns>
        public int CopyFolder(string sourceFolder, string destFolder)
        {
            try
            {
                //如果目标路径不存在,则创建目标路径
                if (!System.IO.Directory.Exists(destFolder))
                {
                    System.IO.Directory.CreateDirectory(destFolder);
                }
                //得到原文件根目录下的所有文件
                string[] files = System.IO.Directory.GetFiles(sourceFolder);
                foreach (string file in files)
                {
                    string name = System.IO.Path.GetFileName(file);
                    string dest = System.IO.Path.Combine(destFolder, name);
                    System.IO.File.Copy(file, dest);//复制文件
                }
                //得到原文件根目录下的所有文件夹
                string[] folders = System.IO.Directory.GetDirectories(sourceFolder);
                foreach (string folder in folders)
                {
                    string name = System.IO.Path.GetFileName(folder);
                    string dest = System.IO.Path.Combine(destFolder, name);
                    CopyFolder(folder, dest);//构建目标路径,递归复制文件
                }
                return 1;
            }
            catch (Exception e)
            {
              //  MessageBox.Show(e.Message);
                return 0;
            }

        }

        [RelayCommand]
        private void Upload_pack()
        {
            Isload = Visibility.Visible;

            // 创建OpenFileDialog实例
            OpenFileDialog openFileDialog = new OpenFileDialog();

            // 设置对话框标题
            openFileDialog.Title = "请选择录像包";

            // 设置默认目录（可选）
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            // 设置过滤条件（例如，仅显示文本文件）
            openFileDialog.Filter = "录像包文件 (*.idvpack)|*.idvpack";

            // 显示对话框并检查用户是否点击了确定按钮
            if (openFileDialog.ShowDialog() == true)
            {
                // 用户选择了文件，获取文件路径
                string filePath = openFileDialog.FileName;
                var fileName=Path.GetFileName(filePath);
                string zipPath = filePath; // ZIP文件的路径
                string extractPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "vfolder",fileName); // 解压的目标目录

                try
                {
                   // string zipPath = @"C:\path\to\your\zipfile.zip"; // 压缩文件路径
                   // string extractPath = @"C:\path\to\extract\directory\"; // 解压目标目录

                    try
                    {
                        // 确保目标目录存在
                        if (!Directory.Exists(extractPath))
                        {
                            Directory.CreateDirectory(extractPath);
                        }

                        // 解压压缩文件到指定目录
                        ZipFile.ExtractToDirectory(zipPath, extractPath);

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"解压过程中发生错误: {ex.Message}");
                    }

                    Showinf("导入成功", "导入成功！", "确定");
                    InitializeViewModel();
                    Isload = Visibility.Hidden;

                }
                catch (Exception ex)
                {
                    Showinf("导入失败", ex.Message, "确定");
                }

            }
        }
        [ObservableProperty]
        private Visibility _isload = Visibility.Hidden;
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



        [RelayCommand]
        private void Delect_video()
        {
            try
            {
                Isload = Visibility.Visible;

                var foldersToCompress = Allvideo.Where(a => a.ischeck == true).Select(a => a._path).ToList();
                foreach (var folder in foldersToCompress)
                {
                    ClearDirectoryContent(folder);
                    Directory.Delete(folder);
                }
                Showinf("删除成功", $"删除成功！\r\n以下录像被删除:\r\n{string.Join("\r\n", foldersToCompress)}", "确定");
                InitializeViewModel();
                Isload = Visibility.Hidden;

            }
            catch (Exception ex)
            {
                Showinf("删除失败", $"删除失败！\r\n原因：{ex.Message}", "确定");

            }

        }


        [RelayCommand]
        private async void Open_useA()
        {
            try
            {
                var con = new use_A(Allvideo.FirstOrDefault(a => a.ischeck == true)._path);
                var messageBox = new Wpf.Ui.Controls.MessageBox
                {

                    Title = "导入到安卓设备",
                    Content = con,

                    ShowInTaskbar = false,
                    Topmost = false,
                    ResizeMode = ResizeMode.NoResize,
                };

                await messageBox.ShowDialogAsync();

            }
            catch (Exception ex)
            {
                Showinf("删除失败", $"删除失败！\r\n原因：{ex.Message}", "确定");

            }

        }





        public static void ClearDirectoryContent(string path)
        {
            // 确保路径指向的是一个存在的目录
            if (Directory.Exists(path))
            {
                // 首先删除所有文件
                string[] files = Directory.GetFiles(path);
                foreach (string file in files)
                {
                    File.Delete(file);
                }

                // 然后递归删除所有子目录
                string[] directories = Directory.GetDirectories(path);
                foreach (string dir in directories)
                {
                    ClearDirectoryContent(dir); // 递归清除子目录内容
                    Directory.Delete(dir, false); // 删除空的子目录
                }

            }
            else
            {
                throw new DirectoryNotFoundException($"指定的路径不存在: '{path}'");
            }
        }

        [RelayCommand]
        private async void open_uploada()
        {
            var con = new Upload_A();
            var messageBox = new Wpf.Ui.Controls.MessageBox
            {

                Title = "从安卓设备导入",
                Content = con,

                ShowInTaskbar = false,
                Topmost = false,
                ResizeMode = ResizeMode.NoResize,
            };

            await messageBox.ShowDialogAsync();


        }


        [RelayCommand]
        private async void check_all()
        {
            var items=new List<DataVideo>();
            if(Allcheck_text == "全选")
            {
                foreach (var item in Allvideo)
                {
                    item.ischeck = true;
                    items.Add(item);
                }
                Allcheck_text = "全不选";
            }
            else
            {
                foreach (var item in Allvideo)
                {
                    item.ischeck = false;
                    items.Add(item);
                }
                Allcheck_text = "全选";

            }
            Allvideo = items;


        }


        [RelayCommand]
        private async void check_video()
        {
          try
            {
                Isload = Visibility.Visible;


                if (DashboardViewModel._User_id == "000")
                {

                    Showinf("警告", "请先选择账号！", "确定");
                    return;
                }

                if (System.Windows.MessageBox.Show( "你是否使用该录像，该操作会覆盖第五人格原本的录像，也就是说你应该提前备份录像","警告", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    if (!Directory.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "vfolder", "back")))
                    {
                        Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "vfolder", "back"));

                    }
                  //  ClearDirectoryContent(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "vfolder", "back"));

                    var foldersToCompress = Allvideo.Where(a => a.ischeck == true).Select(a => a._path).ToList();
                   
                    var mubiaoFolder = $"{File.ReadAllText("GamePath.txt")}\\Documents\\video\\{DashboardViewModel._User_id}\\";
                    // 移动文件夹的所有内容到目标文件夹
                  //  CopyDirectory($"{File.ReadAllText("GamePath.txt")}\\Documents\\video\\{DashboardViewModel._User_id}\\", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "vfolder", "back"));
                    ClearDirectoryContent($"{File.ReadAllText("GamePath.txt")}\\Documents\\video\\{DashboardViewModel._User_id}\\");
                    await Task.Delay(2000);
                
                CopyFolder(foldersToCompress[0],mubiaoFolder);

                

                        Showinf("成功", "使用成功", "确定");

                    InitializeViewModel();


                }

                Isload = Visibility.Hidden;


            }
            catch (Exception ex)
            {
                Showinf("失败", $"失败！\r\n原因：{ex.Message}", "确定");

            }

        }

        private void InitializeViewModel()
        {
            var qPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "vfolder");
            var colorCollection = new List<DataVideo>(); 
            if (!Directory.Exists(qPath))
            {
                Directory.CreateDirectory(qPath);
            }
            var directoryInfo = new DirectoryInfo(qPath);
           var folders = directoryInfo.GetDirectories();
           
            foreach (var item in folders)
            {
                colorCollection.Add(new DataVideo() { _name = item.Name, _number=$"录像数量：{item.GetDirectories().Count().ToString()}", _path = item.FullName,ischeck=false });

            }
            Allvideo = colorCollection;

            _isInitialized = true;
        }
    }
}
