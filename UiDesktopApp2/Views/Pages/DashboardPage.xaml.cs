using System.IO;
using System.Windows;
using System.Windows.Input;
using UiDesktopApp2.ViewModels.Pages;
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
               
                    Content = new SettingsPage(settingsView),
                    PrimaryButtonText = "我知道了",


                    ShowInTaskbar = false,
                    Topmost = false,
                    ResizeMode = ResizeMode.NoResize,
                };
                messageBox.ShowDialogAsync();
            }
               

            InitializeComponent();
           }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
