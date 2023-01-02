using Shared.ExternalServices.APIServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.ExternalServices.Interfaces
{
    public interface ISapService
    {
         SapWalletSapResponse GetWallet(string companyCode, string countryCode, string distributorSapNumber);
        Task<List<SapWalletSapTransactionsResponse>> GetTransactions(int distributorSapAccountId, DateTime fromDate, DateTime calculatedToDate);
    }
}
