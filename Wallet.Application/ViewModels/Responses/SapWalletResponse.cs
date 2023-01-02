using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wallet.Application.ViewModels.Responses
{
    public class SapWalletResponse
    {
        public string StatusCode { get; set; } = "";
        public string Status { get; set; } = "";
        public string Message { get; set; } = "";
        public SapWalletDataVM Data { get; set; } = new SapWalletDataVM();
    }
}
