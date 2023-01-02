using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wallet.Application.Constants
{
    public class EventMessages
    {
        public const string WALLET_TOPIC = "wallets";
        public const string ACCOUNT_TOPIC = "accounts";
        public const string ACCOUNT_SAPACCOUNT_CREATED = "Accounts.SapAccount.Created";
        public const string ACCOUNT_SAPACCOUNT_UPDATED = "Accounts.SAPAccount.Updated";
    }
}
