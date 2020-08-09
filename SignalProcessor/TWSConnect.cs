using System;
using IBBackend;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBApi;
using System.Threading;

namespace SignalProcessor
{
    public class TWSConnect
    {
        //private EReaderMonitorSignal signal = new EReaderMonitorSignal();
        protected IBClient client;

        public IBClient Connect(EReaderMonitorSignal signal)
        {
            int port;
            string host = "127.0.0.1";
            try
            {
                client = new IBClient(signal);
                port = 4111;
                client.Error += new Action<int, int, string, Exception>((id, code, msg, ex) =>
                {
                    Console.Write(msg);
                    if (ex != null)
                        throw ex;
                });

                client.ClientId = 1;
                client.ClientSocket.eConnect(host, port, client.ClientId);

                client.ClientSocket.reqIds(-1);
                Console.WriteLine("connected");

                return client;
            }
            catch (Exception)
            {
                throw new ApplicationException("Please check your connection attributes.");
            }

        }

        public void Disconnect()
        {
            client.ClientSocket.eDisconnect();
        }
    }
}
