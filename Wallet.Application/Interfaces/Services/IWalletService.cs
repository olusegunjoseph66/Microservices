using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wallet.Application.ViewModels.Responses;

namespace Wallet.Application.Interfaces.Services
{
    public interface IWalletService
    {
        Task<SapWalletResponse> GetMySAPWallet(int distributorSapAccountId, bool forceRefresh, CancellationToken cancellationToken);
        Task<SapWalletStatementResponse> GetMySAPWalletStatement(int distributorSapAccountId, DateTime fromDate, DateTime? toDate, CancellationToken cancellationToken);
    }
}
