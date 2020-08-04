using IBApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBBackend
{
    public class ContractManager
    {
        public Contract GetOrderContract()
        {
            Contract contract = new Contract();
            contract.Symbol = "F";
            contract.SecType = "STK";
            contract.Currency = "USD";
            contract.Exchange = "SMART";
            contract.LastTradeDateOrContractMonth = "";

            contract.LocalSymbol = "";
            contract.PrimaryExch = "";
            return contract;
        }
    }
}
