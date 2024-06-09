using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Shapes;

namespace UiDesktopApp2.Views.Pages
{
    /// <summary>
    /// MIT.xaml 的交互逻辑
    /// </summary>
    public partial class MIT : Page
    {
        public MIT()
        {
            InitializeComponent();
            text.Text = "MIT License\r\n\r\nCopyright (c) 2024 第五人格录像管理器\r\n\r\nPermission is hereby granted, free of charge, to any person obtaining a copy\r\nof this software and associated documentation files (the \"Software\"), to deal\r\nin the Software without restriction, including without limitation the rights\r\nto use, copy, modify, merge, publish, distribute, sublicense, and/or sell\r\ncopies of the Software, and to permit persons to whom the Software is\r\nfurnished to do so, subject to the following conditions:\r\n\r\nThe above copyright notice and this permission notice shall be included in all\r\ncopies or substantial portions of the Software.\r\n\r\nTHE SOFTWARE IS PROVIDED \"AS IS\", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR\r\nIMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,\r\nFITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE\r\nAUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER\r\nLIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,\r\nOUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE\r\nSOFTWARE.\r\n";
        }

        private void CardAction_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("explorer.exe", "https://wpfui.lepo.co/");

        }
    }
}
