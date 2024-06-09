using System.Diagnostics;
using UiDesktopApp2.ViewModels.Pages;
using Wpf.Ui.Controls;

namespace UiDesktopApp2.Views.Pages
{
    public partial class SettingsPage : INavigableView<SettingsViewModel>
    {
        public SettingsViewModel ViewModel { get; }

        public SettingsPage(SettingsViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();
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
    }
}
