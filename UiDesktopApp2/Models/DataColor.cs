using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace UiDesktopApp2.Models
{
    public class DataColor : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public string path { get; set; }
        public string time { get; set; }
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
