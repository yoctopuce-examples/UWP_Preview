using System.Diagnostics;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using com.yoctopuce.YoctoAPI;
using System.ComponentModel;
using System.Text;

namespace Demo
{
    public abstract class DemoBase : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }

        private StringBuilder console = new StringBuilder();


        public string Output {
            get {
                return console.ToString();
            }
            set {
                console.Clear();
                console.Append(value);
                OnPropertyChanged(new PropertyChangedEventArgs("Output"));
            }
        }

        protected void WriteLine(string line)
        {
            Debug.WriteLine(line);
            console.Append(line).Append("\n");
            OnPropertyChanged(new PropertyChangedEventArgs("Output"));
        }

        protected void Write(string s)
        {
            Debug.Write(s);
            console.Append(s);
            OnPropertyChanged(new PropertyChangedEventArgs("Output"));
        }

        public abstract Task<int> Run();
    }
}