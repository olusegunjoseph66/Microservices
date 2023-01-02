using Shared.ExternalServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.ExternalServices.APIServices
{
    public class MockSapService:ISapService
    {
        public async Task<List<SapWalletSapTransactionsResponse>> GetTransactions(int distributorSapAccountId, DateTime fromDate, DateTime calculatedToDate)
        {
            var transactionList = new List<SapWalletSapTransactionsResponse>();

            transactionList.Add(

                new SapWalletSapTransactionsResponse
                {
                    Amount = 12345,
                    Description = "Transaction 1",
                    TransactionDate = fromDate,
                    TransactionID = "1",
                     TransactionType=new WalletSapTransactionTypeResponse
                     {
                         Code="001",
                          Name="Cement Purchase"
                     }
                }
                );

            transactionList.Add(

               new SapWalletSapTransactionsResponse
               {
                   Amount = 234567.68M,
                   Description = "Transaction 2",
                   TransactionDate = fromDate,
                   TransactionID = "2",
                   TransactionType = new WalletSapTransactionTypeResponse
                   {
                       Code = "002",
                       Name = "Sugar Purchase"
                   }
               }
               );

            return await Task.FromResult(transactionList);
        }

        public  SapWalletSapResponse GetWallet(string companyCode, string countryCode, string distributorSapNumber)
        {
            return new SapWalletSapResponse { AvailableBalance = 10000000, DistributorNumber=distributorSapNumber, DistributorSapAccountId=0 };
        }
    }
}
