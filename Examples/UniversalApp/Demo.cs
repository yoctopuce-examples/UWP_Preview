using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using com.yoctopuce.YoctoAPI;

namespace Demo
{
    public class Demo : DemoBase
    {
        public Demo(string url, TextBlock textBlock) : base(url, textBlock)
        { }

        public string target { get; set; }
        public string newname { get; set; }

        public override async Task<int> Run()
        {
            try {
                YModule m;

                await YAPI.RegisterHub(_hubULR);

                m = YModule.FindModule(target); // use serial or logical name

                if (await m.isOnline()) {
                    if (!YAPI.CheckLogicalName(newname)) {
                        WriteLine("Invalid name (" + newname + ")");
                        return -1;
                    }

                    await m.set_logicalName(newname);
                    await m.saveToFlash(); // do not forget this

                    Write("Module: serial= " + await m.get_serialNumber());
                    WriteLine(" / name= " + await m.get_logicalName());
                } else {
                    Write("not connected (check identification and USB cable");
                }
            } catch (YAPI_Exception ex) {
                WriteLine("RegisterHub error: " + ex.Message);
            }
            YAPI.FreeAPI();
            return 0;
        }
    }
}