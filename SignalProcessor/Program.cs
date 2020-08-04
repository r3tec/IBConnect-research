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
    class Program
    {
        static void Main(string[] args)
        {
            EReaderMonitorSignal signal = TWSReader.CreateReader();

            TWSConnect conn = new TWSConnect();
            IBClient ibClient = conn.Connect(signal);
            ibClient.ClientSocket.reqIds(-1);

            TWSReader.Start(ibClient);
            Thread.Sleep(1000);
            ContractManager contMan = new ContractManager();
            Contract contract = contMan.GetOrderContract();

            OrderManager ordMan = new OrderManager();
            Order order = ordMan.CreateOrder(0, 0, "BUY", "MKT", 0, 100);
            conn.PlaceOrder(contract, order);
            conn.Disconnect();
            Console.WriteLine("order placed");
            Console.ReadLine();
        }
    }
}
