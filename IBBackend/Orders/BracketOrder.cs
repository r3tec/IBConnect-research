using IBApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBBackend.Orders
{
    public class BracketOrder
    {
        public static List<Order> CreateBracketOrder(
            int parentId, 
            string action, 
            double quantity, 
            double limitPrice,
           double tplPrice, 
           double slPrice)
        {
            //This will be our main or "parent" order
            Order parent = new Order();
            parent.OrderId = parentId;
            parent.Action = action;
            parent.OrderType = "LMT";
            parent.TotalQuantity = quantity;
            parent.LmtPrice = limitPrice;
            //The parent and children orders will need this attribute set to false to prevent accidental executions.
            //The LAST CHILD will have it set to true, 
            parent.Transmit = false;
            Order takeProfit = new Order();
            takeProfit.OrderId = parent.OrderId + 1;
            takeProfit.Action = action.Equals("BUY") ? "SELL" : "BUY";
            takeProfit.OrderType = "LMT";
            takeProfit.TotalQuantity = quantity;
            takeProfit.LmtPrice = tplPrice;
            takeProfit.ParentId = parentId;
            takeProfit.Transmit = false;
            Order stopLoss = new Order();
            stopLoss.OrderId = parent.OrderId + 2;
            stopLoss.Action = action.Equals("BUY") ? "SELL" : "BUY";
            stopLoss.OrderType = "STP";
            //Stop trigger price
            stopLoss.AuxPrice = slPrice;
            stopLoss.TotalQuantity = quantity;
            stopLoss.ParentId = parentId;
            //In this case, the low side order will be the last child being sent. Therefore, it needs to set this attribute to true 
            //to activate all its predecessors
            stopLoss.Transmit = true;
            List<Order> bracketOrder = new List<Order>();
            bracketOrder.Add(parent);
            bracketOrder.Add(takeProfit);
            bracketOrder.Add(stopLoss);
            return bracketOrder;
        }
    }
}
