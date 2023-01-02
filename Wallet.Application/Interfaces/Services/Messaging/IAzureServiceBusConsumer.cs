using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wallet.Application.Interfaces.Services.Messaging
{
    public interface IAzureServiceBusConsumer
    {
        Task StartAutoUpdateDistributorAccountEvent();
        Task StopAutoUpdateDistributorAccountEvent();
    }
}
