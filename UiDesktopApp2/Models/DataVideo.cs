using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace UiDesktopApp2.Models
{
    public class DataVideo : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public string _path { get; set; }
        public string _name { get; set; }
        public string _number { get; set; }

        public bool _ischeck { get; set; }
        public bool ischeck
        {
            get => _ischeck;
            set
            {
                if (_ischeck != value)
                {
                    _ischeck = value;
                    OnPropertyChanged(); // 使用 CallerMemberName 自动获取属性名
                }
            }
        }

    }


    public class DataCloudVideo : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public int id { get; set; }
        public string Video_msg { get; set; }
        public string Video_url { get; set; }
        public string Video_name { get; set; }

        public bool _ischeck { get; set; }
        public bool ischeck
        {
            get => _ischeck;
            set
            {
                if (_ischeck != value)
                {
                    _ischeck = value;
                    OnPropertyChanged(); // 使用 CallerMemberName 自动获取属性名
                }
            }
        }

    }

}
