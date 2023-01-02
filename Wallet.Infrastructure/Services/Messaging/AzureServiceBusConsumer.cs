using Azure.Messaging.ServiceBus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Shared.Data.Models;
using Shared.Data.Repository;
using Shared.ExternalServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wallet.Application.Constants;
using Wallet.Application.DTOs.Events;
using Wallet.Application.Interfaces.Services.Messaging;

namespace Wallet.Infrastructure.Services.Messaging
{
    public class AzureServiceBusConsumer : IAzureServiceBusConsumer
    {
        private IAsyncRepository<DistributorSapAccount> _distributorSapNo;
        private ServiceBusProcessor accountProcessor;
        public readonly IServiceScopeFactory _scopeFactory;
        public readonly IMessagingService _messageBus;
        public AzureServiceBusConsumer(IAsyncRepository<DistributorSapAccount> distributorSapNo,
            IServiceScopeFactory scopeFactory, IMessagingService messageBus)
        {
            _messageBus = messageBus;
            _distributorSapNo = distributorSapNo;
            _scopeFactory = scopeFactory;
            accountProcessor = _messageBus.ConsumeMessage(EventMessages.ACCOUNT_TOPIC, EventMessages.WALLET_TOPIC);
        }
                     
        public async Task StartAutoUpdateDistributorAccountEvent()
        {
            accountProcessor.ProcessMessageAsync += OnAutoDistributorAccount;
            accountProcessor.ProcessErrorAsync += ErrorHandler;
            await accountProcessor.StartProcessingAsync();
        }

        public async Task StopAutoUpdateDistributorAccountEvent()
        {
            await accountProcessor.StopProcessingAsync();
            await accountProcessor.DisposeAsync();
        }

        #region Private Handlers
        Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }
        #endregion

        #region Operational Function
        private async Task OnAutoDistributorAccount(ProcessMessageEventArgs args)
        {
            var subject = args.Message.Subject;
            if (subject.ToLower() == EventMessages.ACCOUNT_SAPACCOUNT_CREATED.ToLower())
            {
                var message = args.Message;
                var body = Encoding.UTF8.GetString(message.Body);

                var distributorAccount = JsonConvert.DeserializeObject<AccountsSapAccountCreatedMessage>(body);
                using var scope = _scopeFactory.CreateScope();
                _distributorSapNo = scope.ServiceProvider.GetRequiredService<IAsyncRepository<DistributorSapAccount>>();

                var sapAccountDetail = await _distributorSapNo.Table.FirstOrDefaultAsync(c =>
                c.DistributorSapAccountId == distributorAccount.SapAccountId && c.UserId == distributorAccount.UserId);
                if (sapAccountDetail == null)
                {
                    DistributorSapAccount sapAccount = new()
                    {
                        AccountType = distributorAccount.AccountType.Name,
                        DateRefreshed = DateTime.Now,
                        DistributorName = distributorAccount.DistributorName,
                        CompanyCode = distributorAccount.CompanyCode,
                        CountryCode = distributorAccount.CountryCode,
                        UserId = distributorAccount.UserId,
                        DistributorSapNumber = distributorAccount.DistributorSapNumber,
                        DistributorSapAccountId = distributorAccount.SapAccountId
                    };

                    await _distributorSapNo.AddAsync(sapAccount);
                    await _distributorSapNo.CommitAsync(default);
                }

            }
            else if (subject.ToLower() == EventMessages.ACCOUNT_SAPACCOUNT_UPDATED.ToLower())
            {
                var message = args.Message;
                var body = Encoding.UTF8.GetString(message.Body);
                var disAccount = JsonConvert.DeserializeObject<AccountsSapAccountCreatedMessage>(body);

                var sapAccountDetail = await _distributorSapNo.Table.FirstOrDefaultAsync(c => c.DistributorSapAccountId == disAccount.SapAccountId);
                if (sapAccountDetail != null)
                {
                    sapAccountDetail = new DistributorSapAccount()
                    {
                        AccountType = disAccount.AccountType.Name,
                        DateRefreshed = disAccount.DateModified,
                        DistributorName = disAccount.DistributorName,
                        CompanyCode = disAccount.CompanyCode,
                        CountryCode = disAccount.CountryCode,
                        UserId = disAccount.UserId,
                        DistributorSapNumber = disAccount.DistributorSapNumber,
                    };
                    using var scope = _scopeFactory.CreateScope();
                    _distributorSapNo = scope.ServiceProvider.GetRequiredService<IAsyncRepository<DistributorSapAccount>>();

                    _distributorSapNo.Update(sapAccountDetail);
                    await _distributorSapNo.CommitAsync(default);
                }
            }

        }
        #endregion
    }
}
