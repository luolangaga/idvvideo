
using UiDesktopApp2.ViewModels.Windows;
using Wpf.Ui;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;
using AutoUpdaterDotNET;
using System.Diagnostics;

namespace UiDesktopApp2.Views.Windows
{
    public partial class MainWindow :  INavigationWindow
    {
        public MainWindowViewModel ViewModel { get; }

        public MainWindow(
            MainWindowViewModel viewModel,
            IPageService pageService,
            INavigationService navigationService
        )
        {
            ViewModel = viewModel;
            DataContext = this;

            SystemThemeWatcher.Watch(this);

            InitializeComponent();
            SetPageService(pageService);

            navigationService.SetNavigationControl(RootNavigation);
            AutoUpdater.AppTitle = "第五人格录像管理器更新";
         
            AutoUpdater.Start("https://gitee.com/luolan1/idvvideo/releases/download/xml/AutoUpdaterStarter.xml");
        }

        #region INavigationWindow methods

        public INavigationView GetNavigation() => RootNavigation;

        public bool Navigate(Type pageType) => RootNavigation.Navigate(pageType);

        public void SetPageService(IPageService pageService) => RootNavigation.SetPageService(pageService);

        public void ShowWindow() => Show();

        public void CloseWindow() => Close();

        #endregion INavigationWindow methods

        /// <summary>
        /// Raises the closed event.
        /// </summary>
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            KillProcess("adb");
            // Make sure that closing this window will begin the process of closing the application.
            Application.Current.Shutdown();
        }
        public static void KillProcess(string Pname)
        {
            foreach (var v in Process.GetProcessesByName(Pname))
            {
                v.Kill();
            }
        }

        INavigationView INavigationWindow.GetNavigation()
        {
            throw new NotImplementedException();
        }

        public void SetServiceProvider(IServiceProvider serviceProvider)
        {
            throw new NotImplementedException();
        }
    }
}
