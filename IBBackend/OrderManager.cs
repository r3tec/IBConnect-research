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
        public Order CreateOrder(int orderId, 
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
