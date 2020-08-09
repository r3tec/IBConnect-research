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
        protected IBClient ibClient;

        public IBClient Connect(EReaderMonitorSignal signal)
        {
            int port;
            string host = "127.0.0.1";
            try
            {
                ibClient = new IBClient(signal);
                port = 4002;

                ibClient.ClientId = 1;
                ibClient.ClientSocket.eConnect(host, port, ibClient.ClientId);
                Console.WriteLine("connected");
                return ibClient;
            }
            catch (Exception)
            {
                throw new ApplicationException("Please check your connection attributes.");
            }

        }

        public void PlaceOrder(Contract contract, Order order)
        {
            if (order.OrderId != 0)
            {
                ibClient.ClientSocket.placeOrder(order.OrderId, contract, order);
            }
            else
            {
                ibClient.ClientSocket.placeOrder(ibClient.NextOrderId, contract, order);
                ibClient.NextOrderId++;
            }
        }

        public void Disconnect()
        {
            ibClient.ClientSocket.eDisconnect();
        }
    }
}
