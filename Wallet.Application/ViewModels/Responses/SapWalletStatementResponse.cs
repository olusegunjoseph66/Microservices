using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wallet.Application.ViewModels.Responses
{
    public class SapWalletStatementResponse
    {
        public string StatusCode { get; set; } = "";
        public string Status { get; set; } = "";
        public string Message { get; set; } = "";

        public SapWalletStatementResponseItem Data { get; set; } = new SapWalletStatementResponseItem();
    }
}
