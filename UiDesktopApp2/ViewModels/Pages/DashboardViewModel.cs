using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Runtime.CompilerServices;
using System.Security.Permissions;
using System.Text;
using System.Xml.Linq;
using UiDesktopApp2.Models;
using UiDesktopApp2.Views.Pages;
using Wpf.Ui.Controls;

namespace UiDesktopApp2.ViewModels.Pages
{
   
    public partial class DashboardViewModel : ObservableObject
    {

        public event PropertyChangedEventHandler PropertyChanged;
        protected new virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }



        public void OnNavigatedTo()
        {
            InitializeViewModel();
        }
        [ObservableProperty]
        private int _counter = 0;
        [ObservableProperty]
        private Visibility _isload = Visibility.Hidden;
        [ObservableProperty]
        public static string _User_id="000";
       [ObservableProperty]
        public static IEnumerable<DataColor>? _colors= new List<DataColor>() ;

        [ObservableProperty]
        public  List<string> _Game_User= Get_userid();
        //获取Documents\video\游戏id目录下的文件夹名


        public static List<DataColor> Check_video = new List<DataColor>();

        public static List<string> Get_userid()
        {
            try
            {
                List<string> Game_User=new List<string>();
                var baseDirectoryPath = $"{File.ReadAllText("GamePath.txt")}\\Documents\\video";
                string[] subDirectories = Directory.GetDirectories(baseDirectoryPath);

                Console.WriteLine("Relative Names of Subdirectories:");
                foreach (string subDirectory in subDirectories)
                {
                    // 获取子目录相对于基础目录的路径
                    string relativePath = subDirectory.Substring(baseDirectoryPath.Length + 1); // +1 to remove the trailing '\'
                    Game_User.Add(relativePath);   
                }


                return Game_User;
            }
            catch (Exception ex)
            {
                return null;
            }
           
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
        [ObservableProperty]
        private string _allcheck_text = "全选";

        [RelayCommand]
        private async void check_all()
        {
            var items = new List<DataColor>();
            if (Allcheck_text == "全选")
            {
                foreach (var item in Colors)
                {
                    item.ischeck = true;
                    items.Add(item);
                }
                Allcheck_text = "全不选";
            }
            else
            {
                foreach (var item in Colors)
                {
                    item.ischeck = false;
                    items.Add(item);
                }
                Allcheck_text = "全选";

            }
            Colors = items;


        }

        [RelayCommand]
        private void OnRestart()
        {

            try
            {
                var data = new List<DataColor>();
                var Game_User = GetFoldersWithLastWriteTime($"{File.ReadAllText("GamePath.txt")}\\Documents\\video\\{User_id}\\");
                foreach (var (Name, LastWriteTime) in Game_User)
                {
                    data.Add(new DataColor() { path = Name, time = LastWriteTime.ToString("F"), ischeck = false });

                }
               Colors = data;
            }
            catch (Exception)
            {
                if (User_id=="000")
                {
                    Showinf("错误", "请先选择游戏id", "确定");
                }
                else
                {
                    Showinf("错误", "未找到录像文件夹", "确定");
                }

            }
            
           
        }


        [RelayCommand]
        private void Delect_video()
        {
            try
            {
                var foldersToCompress = Colors.Where(a => a.ischeck == true).Select(a => a.path).ToList();
                foreach (var folder in foldersToCompress)
                {
                    Directory.Delete(folder, true);
                }
                Showinf("删除成功", $"删除成功！\r\n以下录像被删除:\r\n{string.Join("\r\n",foldersToCompress)}", "确定");
              
                OnRestart();
            }
            catch (Exception ex)
            {
                Showinf("删除失败", $"删除失败！\r\n原因：{ex.Message}", "确定");

            }

        }






        [RelayCommand]
        private void Build_vfolder()
        {
            try
            {
                var messageBox = new Wpf.Ui.Controls.MessageBox
                {

                    Title = "新建录像夹",
                    Content = new build_floder("录像夹"),

                    ShowInTaskbar = false,
                    Topmost = false,
                    ResizeMode = ResizeMode.NoResize,
                };
                messageBox.ShowDialogAsync();
            }
            catch (Exception ex)
            {
                Showinf("构建失败！", $"构建失败！\r\n原因：{ex.Message}", "确定");

            }

        }


        private static void CopyDirectory(string sourcePath, string targetPath)
        {
            // 获取源文件夹中的所有文件和子文件夹
            string[] files = Directory.GetFiles(sourcePath);
            string[] dirs = Directory.GetDirectories(sourcePath);

            // 复制所有文件
            foreach (string file in files)
            {
                string fileName = Path.GetFileName(file);
                string destFile = Path.Combine(targetPath, fileName);
                File.Copy(file, destFile, true);
            }

            // 递归复制所有子文件夹
            foreach (string dir in dirs)
            {
                string subdir = Path.GetFileName(dir);
                string destDir = Path.Combine(targetPath, subdir);
                CopyDirectory(dir, destDir);
            }
        }




        private async void Showinf(string Title,object cont,string button_text)
        {
            var messageBox = new Wpf.Ui.Controls.MessageBox
            {

                Title = Title,
                Content = cont
                //new TextBlock
                //{
                //    Text = cont,
                //    TextWrapping = TextWrapping.Wrap,
                //}
                ,
                PrimaryButtonText = button_text,


                ShowInTaskbar = false,
                Topmost = false,
                ResizeMode = ResizeMode.NoResize,
            };
         
            await messageBox.ShowDialogAsync();
        }
        [RelayCommand]
        private async void Buildpack()
        {

            Isload = Visibility.Visible;
            // 要压缩的文件夹列表

            try
            {
                var messageBox = new Wpf.Ui.Controls.MessageBox
                {

                    Title = "打包",
                    Content = new build_floder("录像"),

                    ShowInTaskbar = false,
                    Topmost = false,
                    ResizeMode = ResizeMode.NoResize,
                };
               await messageBox.ShowDialogAsync();
            }
            catch (Exception ex)
            {
                Showinf("构建失败！", $"构建失败！\r\n原因：{ex.Message}", "确定");

            }

            await Task.Delay(1000);
            Isload = Visibility.Hidden;
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
        private void InitializeViewModel()
        {
           
        }
    }
}
