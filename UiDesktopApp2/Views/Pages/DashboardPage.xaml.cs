using Flurl.Http;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography.Xml;
using System.Text.Json;
using System.Windows;
using System.Windows.Input;
using UiDesktopApp2.ViewModels.Pages;
using UiDesktopApp2.Views.Windows;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace UiDesktopApp2.Views.Pages
{
    public partial class DashboardPage : INavigableView<DashboardViewModel>
    {
        public DashboardViewModel ViewModel { get; }

        public  DashboardPage(DashboardViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;
            if (File.ReadAllText("GamePath.txt") == "")
            {
                SettingsViewModel settingsView= new SettingsViewModel();
                var messageBox = new Wpf.Ui.Controls.MessageBox
                {

                    Title = "请先到设置界面配置第五人格的路径",
               
                    Content = new SettingsPage(settingsView,false),
                    PrimaryButtonText = "我知道了",


                    ShowInTaskbar = false,
                    Topmost = false,
                    ResizeMode = ResizeMode.NoResize,
                };
                messageBox.ShowDialogAsync();
            }
               

            InitializeComponent();
         //   Task.Run(async () => {
             download();

            //    });
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
        private async void download()
        {
          
            try
            {
                int a = 0;
              
                string NR = Clipboard.GetText().Split("|")[0];
                //Showinf("提示", NR, "确认");

                if (NR.Contains(Config.http_url))
                {
                    var weatherForecast = JsonSerializer.Deserialize<List<string>>(NR);
                    Download d=new Download(weatherForecast);
                    d.Show();

                   
                    
                }

              
            }
            catch (Exception ex)
            {

               // Showinf("成功", $"共导入{ex.Message}个录像", "确定");
            }
        }
        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
