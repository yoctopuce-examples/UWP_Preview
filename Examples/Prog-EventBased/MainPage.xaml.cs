using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using com.yoctopuce.YoctoAPI;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Prog_EventBased
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private DispatcherTimer timer;

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



        async Task anButtonValueChangeCallBack(YFunction fct, string value)
        {
            Output.Text += await fct.get_hardwareId() + ": " + value + " (new value)\n";
        }

        async Task sensorValueChangeCallBack(YSensor fct, string value)
        {
            Output.Text += await fct.get_hardwareId() + ": " + value + " (new value)\n";
        }

        async Task sensorTimedReportCallBack(YSensor fct, YMeasure measure)
        {
            Output.Text += await fct.get_hardwareId() + ": " + measure.get_averageValue() + " " + await fct.get_unit() + " (timed report)\n";
        }

        async Task deviceLog(YModule module, string logline)
        {
            Output.Text += "log:" + await module.get_hardwareId() + ":" + logline;
        }

        async Task deviceArrival(YModule m)
        {
            string serial = await m.get_serialNumber();
            Output.Text += "Device arrival : " + serial + "\n";
            await m.registerLogCallback(deviceLog);

            // First solution: look for a specific type of function (eg. anButton)
            int fctcount = await m.functionCount();
            for (int i = 0; i < fctcount; i++) {
                string hardwareId = serial + "." + await m.functionId(i);
                if (hardwareId.IndexOf(".anButton") >= 0) {
                    Output.Text += "- " + hardwareId + "\n";
                    YAnButton anButton = YAnButton.FindAnButton(hardwareId);
                    await anButton.registerValueCallback(anButtonValueChangeCallBack);
                }
            }

            // Alternate solution: register any kind of sensor on the device
            YSensor sensor = YSensor.FirstSensor();
            while (sensor != null) {
                YModule module = await sensor.get_module();
                if (await module.get_serialNumber() == serial) {
                    string hardwareId = await sensor.get_hardwareId();
                    Output.Text += "- " + hardwareId + "\n";
                    await sensor.registerValueCallback(sensorValueChangeCallBack);
                    await sensor.registerTimedReportCallback(sensorTimedReportCallBack);
                }
                sensor = sensor.nextSensor();
            }
        }

        async Task deviceRemoval(YModule m)
        {
            Output.Text += "Device removal : " + await m.get_serialNumber() + "\n";
        }

        public async void Each_Tick(object sender, object o)
        {
            DispatcherTimer timer = (DispatcherTimer) sender;
            timer.Stop();
            try {
                int res = await YAPI.UpdateDeviceList();
                await YAPI.HandleEvents();
            } catch (YAPI_Exception ex) {
                Output.Text += "Error:" + ex.Message;
                throw;
            }
            timer.Start();

        }

        private bool started = false;

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!started) {
                string url = this.url.Text;
                try {
                    YAPI.RegisterLogFunction(yoctoLog);
                    await YAPI.RegisterHub(url);
                    YAPI.RegisterDeviceArrivalCallback(deviceArrival);
                    YAPI.RegisterDeviceRemovalCallback(deviceRemoval);
                } catch (YAPI_Exception ex) {
                    Output.Text = "Error:" + ex.Message + "\n";
                    return;
                }
                timer = new DispatcherTimer();
                timer.Interval = new TimeSpan(0, 0, 0, 0, 100); // 100 Milliseconds 
                timer.Tick += new EventHandler<object>(Each_Tick);
                timer.Start();
                Output.Text = "Init done:\n";
                initButton.Content = "Stop";
                started = true;
            } else {
                timer.Stop();
                YAPI.FreeAPI();
                initButton.Content = "Start";
                started = false;
            }
        }

    }
}
