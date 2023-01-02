using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.ExternalServices.APIServices
{
    public class SapWalletSapTransactionsResponse
    {
        public string TransactionID { get; set; } = "";

        public decimal Amount { get; set; }

        public string Description { get; set; } = "";

        public DateTime TransactionDate { get; set; }

        public WalletSapTransactionTypeResponse TransactionType { get; set; } = new WalletSapTransactionTypeResponse();
    }
}
