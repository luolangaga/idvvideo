using System.Collections.ObjectModel;
using Wpf.Ui.Controls;

namespace UiDesktopApp2.ViewModels.Windows
{
    public partial class MainWindowViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _applicationTitle = "第五人格录像管理器";

        [ObservableProperty]
        private ObservableCollection<object> _menuItems = new()
        {
            new NavigationViewItem()
            {
                Content = "录像",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Video24 },
                TargetPageType = typeof(Views.Pages.DashboardPage)
            },
            new NavigationViewItem()
            {
                Content = "录像夹",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Folder24 },
                TargetPageType = typeof(Views.Pages.DataPage)
            },
            new NavigationViewItem()
            {
                Content = "社区",
                Icon = new SymbolIcon { Symbol = SymbolRegular.CompassNorthwest24 },
                TargetPageType = typeof(Views.Pages.Cloudvideo)
            },
            new NavigationViewItem()
            {
                Content = "登录器",
                Icon = new SymbolIcon { Symbol = SymbolRegular.QrCode24 },
                TargetPageType = typeof(Views.Pages.idvlogin)
            }
        };

        [ObservableProperty]
        private ObservableCollection<object> _footerMenuItems = new()
        {
            new NavigationViewItem()
            {
                Content = "设置",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Settings24 },
                TargetPageType = typeof(Views.Pages.SettingsPage)
            }
        };

        [ObservableProperty]
        private ObservableCollection<MenuItem> _trayMenuItems = new()
        {
            new MenuItem { Header = "Home", Tag = "tray_home" }
        };
    }
}
