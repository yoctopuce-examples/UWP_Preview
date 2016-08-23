using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System.Threading;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using com.yoctopuce.YoctoAPI;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UniversalApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }
#pragma warning disable 1998

        private async Task yoctoLog(string line)
        {
            int thid = Environment.CurrentManagedThreadId;
            Debug.Write("[" + thid + "]:" + line);
            //Output.Text += "YAPI:"+line;
        }
#pragma warning restore 1998

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            try {
                int res = await YAPI.UpdateDeviceList();
                YModule module = YModule.FirstModule();
                while (module != null) {
                    Output.Text += await module.get_serialNumber() + " (" + await module.get_upTime() + ")\n";
                    int beacon = await module.get_beacon();
                    if (beacon == YModule.BEACON_ON) {
                        await module.set_beacon(YModule.BEACON_OFF);
                    } else {
                        await module.set_beacon(YModule.BEACON_ON);
                    }
                    module = module.nextModule();
                }
            } catch (YAPI_Exception ex) {
                Output.Text += "Error:" + ex.Message;
                throw;
            }
        }

        private async void Button_Click_init(object sender, RoutedEventArgs e)
        {
            string url = this.url.Text;

            await yoctoLog("Main thread");
            YAPI.RegisterLogFunction(yoctoLog);
            try {
                int res = await YAPI.RegisterHub(url);
                Output.Text = "Init done:\n";
                initButton.IsEnabled = false;
                enumButton.IsEnabled = true;
                freebutton.IsEnabled = true;

            } catch (YAPI_Exception ex) {
                Output.Text = "Error:" + ex.Message + "\n";
            }

        }

        private void Button_Click_free(object sender, RoutedEventArgs e)
        {
            YAPI.FreeAPI();
            initButton.IsEnabled = true;
            enumButton.IsEnabled = false;
            freebutton.IsEnabled = false;
        }
    }
}
