using IBApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBBackend
{
    public class OrderManager
    {
        private static IBClient ibClient;

        public static void StartOrderManager(IBClient ibc) { ibClient = ibc; }
        public static void RequestOrders()
        {
            ibClient.ClientSocket.reqAllOpenOrders();
        }

        public static void PlaceOrder(Contract contract, Order order)
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

        public static Order CreateOrder(int orderId, 
            int parentOrderId, 
            string action, 
            string orderType,
            double lmtPrice,
            double quantity)
        {
            Order order = new Order();
            if (orderId != 0)
                order.OrderId = orderId;
            if (parentOrderId != 0)
                order.ParentId = parentOrderId;
            order.Action = action;
            order.OrderType = orderType;
            if(lmtPrice > 0)
                order.LmtPrice = lmtPrice;
            order.TotalQuantity = quantity;
            order.Tif = "DAY";
            //order.Account = account.Text;
            //order.ModelCode = modelCode.Text;
            //if (!auxPrice.Text.Equals(""))
            //    order.AuxPrice = Double.Parse(auxPrice.Text);
            //if (!displaySize.Text.Equals(""))
            //    order.DisplaySize = Int32.Parse(displaySize.Text);
            //if (!cashQty.Text.Equals(""))
            //    order.CashQty = Double.Parse(cashQty.Text);

            return order;
        }

    }
}
