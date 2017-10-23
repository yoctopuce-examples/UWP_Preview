using System.Diagnostics;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using com.yoctopuce.YoctoAPI;

namespace Demo
{
    public abstract class DemoBase
    {
        protected string _hubULR;
        private TextBlock _textBlock;

        public DemoBase(string url, TextBlock textBlock)
        {
            this._hubULR = url;
            _textBlock = textBlock;
            textBlock.Text = "";
        }

        protected void WriteLine(string line)
        {
            Debug.WriteLine(line);
            _textBlock.Text += line + "\n";
        }

        protected void Write(string s)
        {
            Debug.Write(s);
            _textBlock.Text += s;
        }

        public abstract Task<int> Run();
    }
}