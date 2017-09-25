using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
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








        public ulong greftime;
        public StringBuilder dump = new StringBuilder();
        public ulong sum1, sum2, cnt1, cnt2;

        async Task callback(YAnButton input, string value)
        {
            int val = int.Parse(value);
            if (val < 10 || val > 995) {
                sum2 += (YAPI.GetTickCount() - greftime);
                cnt2++;
                dump.AppendLine(YAPI.GetTickCount() - greftime + ":" + value);
            }
        }





        private async void Button_Click_perf(object sender, RoutedEventArgs e)
        {
            try {
                YModule m;
                YRelay relay1;
                YAnButton input1;
                int i;
                int sum1;
                int cnt1;
                ulong reftime;
                long pulse_counter;

                Output.Text = "  Using lib " + YAPIContext.GetAPIVersion() + "\n";
                input1 = YAnButton.FindAnButton("input1");
                if (!(await input1.isOnline())) {
                    Output.Text = "No AnButton named 'input1' found, check cable!";
                    return;
                }
                m = await input1.get_module();
                Output.Text += "  Use Knob " + await m.get_serialNumber() + " (" + await m.get_firmwareRelease() + ")\n";
                relay1 = YRelay.FindRelay("relay1");
                if (!(await input1.isOnline()) || !(await relay1.isOnline())) {
                    Output.Text = "No Relay named 'relay1' found, check cable!";
                    return;
                }
                m = await relay1.get_module();
                await input1.resetCounter();
                Output.Text += "  Use Relay " + await m.get_serialNumber() + " (" + await m.get_firmwareRelease() + ")\n";
                reftime = YAPIContext.GetTickCount();
                i = 0;
                while (i < 64) {
                    await relay1.set_state(YRelay.STATE_B);
                    await relay1.set_state(YRelay.STATE_A);
                    i = i + 1;
                }
                pulse_counter = await input1.get_pulseCounter();
                if (pulse_counter != 128) {
                    Output.Text += "ERROR: puse counter = " + Convert.ToString(pulse_counter) + "\n";
                    return;
                }

                reftime = YAPIContext.GetTickCount() - reftime;
                Output.Text += "   - Average 'set'     time: " + Convert.ToString((int)reftime) + " / 128 = " +
                               YAPIContext.imm_floatToStr((double)reftime / 128.0) + "ms\n";
                await YAPI.Sleep(3000);
                reftime = YAPIContext.GetTickCount();
                i = 0;
                while (i < 32) {
                    if (((i) & (1)) == 1) {
                        await relay1.set_state(YRelay.STATE_A);
                    } else {
                        await relay1.set_state(YRelay.STATE_B);
                    }
                    await relay1.get_state();
                    i = i + 1;
                }
                reftime = YAPIContext.GetTickCount() - reftime;
                Output.Text += "   - Average 'set/get' time: " + Convert.ToString((int)reftime) + " / 32 = " +
                               YAPIContext.imm_floatToStr((double)reftime / 32) + "ms\n";
                await input1.set_pulseCounter(0);
                await input1.registerValueCallback(callback);
                await YAPI.Sleep(3000);
                sum1 = 0;
                cnt1 = 0;
                sum2 = 0;
                cnt2 = 0;
                i = 0;
                while (i < 32) {
                    reftime = YAPIContext.GetTickCount();
                    if (((i) & (1)) == 1) {
                        await relay1.set_state(YRelay.STATE_A);
                    } else {
                        await relay1.set_state(YRelay.STATE_B);
                    }
                    greftime = YAPIContext.GetTickCount();
                    reftime = greftime - reftime;
                    cnt1 = cnt1 + 1;
                    sum1 = sum1 + (int)reftime;
                    await YAPI.Sleep(50);
                    i = i + 1;
                }
                Output.Text += "   - Average command time:   " + Convert.ToString(sum1) + " / " + Convert.ToString(cnt1) +
                               " = " + YAPIContext.imm_floatToStr((double)sum1 / cnt1) + "ms\n";
                Output.Text += "   - Average round-trip time:" + YAPIContext.imm_floatToStr((double)sum2 / cnt2) + " (on " + cnt2 + " samples)\n";
                pulse_counter = await input1.get_pulseCounter();
                Output.Text += "   - puse counter = " + Convert.ToString(pulse_counter) + "\n";
            } catch (YAPI_Exception ex) {
                Output.Text += "Error:" + ex.Message;
            }
        }
    }
}
