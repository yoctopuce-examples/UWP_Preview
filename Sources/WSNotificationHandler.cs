using System.Threading.Tasks;

namespace com.yoctopuce.YoctoAPI {

    internal class WSNotificationHandler : NotificationHandler {
        public WSNotificationHandler(YHTTPHub hub, object callbackSession) : base(hub) {
            //TODO: implement WebSocket
            throw new YAPI_Exception(YAPI.NOT_SUPPORTED, "This yoctolib does not support WebSocket. Use yoctolib-jEE");
        }


        internal override Task<byte[]> hubRequestSync(string req_first_line, byte[] req_head_and_body, uint mstimeout)
        {
            throw new System.NotImplementedException();
        }

        internal override  Task<byte[]> devRequestSync(YDevice device, string req_first_line, byte[] req_head_and_body, uint mstimeout,
            YGenericHub.RequestProgress progress, object context)
        {
            throw new System.NotImplementedException();
        }

        internal override Task devRequestAsync(YDevice device, string req_first_line, byte[] req_head_and_body, YGenericHub.RequestAsyncResult asyncResult,
            object asyncContext)
        {
            throw new System.NotImplementedException();
        }

        internal override Task<bool> waitAndFreeAsyncTasks(ulong timeout)
        {
            throw new System.NotImplementedException();
        }

        public override bool Connected { get; }
        public override bool hasRwAccess()
        {
            throw new System.NotImplementedException();
        }

      

        public override Task Start()
        {
            throw new System.NotImplementedException();
        }
    }

}