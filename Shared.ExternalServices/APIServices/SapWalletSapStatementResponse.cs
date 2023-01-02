using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.ExternalServices.APIServices
{
    public class SapWalletSapStatementResponse
    {
        public string WalletId { get; set; } = "";
        public decimal AvailableBalance { get; set; }
        public DateTime FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public List<SapWalletSapTransactionsResponse> Transactions = new();
    }
}
