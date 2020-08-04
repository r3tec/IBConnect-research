using IBApi;
using IBBackend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SignalProcessor
{
    public class TWSReader
    {
        private static EReaderMonitorSignal signal = new EReaderMonitorSignal();

        private TWSReader() { }
        public static EReaderMonitorSignal CreateReader()
        {
            return signal;
        }
        public static void Start(IBClient ibClient)
        {
            var reader = new EReader(ibClient.ClientSocket, signal);
            reader.Start();
            new Thread(() => { 
                while (ibClient.ClientSocket.IsConnected()) 
                { 
                    signal.waitForSignal(); 
                    reader.processMsgs(); 
                }
                Console.WriteLine("End of listen");
            }) { IsBackground = true }.Start();
        }
    }
}
