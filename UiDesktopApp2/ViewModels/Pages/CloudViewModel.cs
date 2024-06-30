using Flurl.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using UiDesktopApp2.Models;
using UiDesktopApp2.Views.Pages;
using UiDesktopApp2.Views.Windows;

namespace UiDesktopApp2.ViewModels.Pages
{
    public partial class CloudViewModel : ObservableObject
    {
        public CloudViewModel()
        {
            InitializeViewModel();
        }

        [ObservableProperty]
        public static IEnumerable<DataCloudVideo>? _cloudvideo = new List<DataCloudVideo>();
        [ObservableProperty]
        public static Visibility _isload = Visibility.Hidden;

        public void OnNavigatedTo()
        {
            InitializeViewModel();
        }
        private async void InitializeViewModel()
        {
            Isload = Visibility.Hidden;
           var a= await $"{Config.http_url}/api/v1/videos".GetJsonAsync<List<DataCloudVideo>>();
            Cloudvideo = a;
            Isload = Visibility.Visible;
        }
        [RelayCommand]
        public void download()
        {
          var a=  _cloudvideo.FirstOrDefault(a => a.ischeck == true);
            var uuidN = Guid.NewGuid().ToString("N");
            // 生成随机的GUID作为文件名
            string guidFileName = $"{uuidN}.idvpack";
             Random ran = new Random();
            string n = ran.Next(9999999).ToString();
            Download download = new Download(JsonSerializer.Deserialize<List<string>>(a.Video_url), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "pack", guidFileName), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "vfolder", $"share{n}"));
            download.Show();
        }
        [RelayCommand]
        public void Restart()
        {
            InitializeViewModel();
        }
    }
    }
