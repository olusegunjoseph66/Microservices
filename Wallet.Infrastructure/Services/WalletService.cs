using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Shared.Data.Repository;
using Shared.ExternalServices.APIServices;
using Shared.ExternalServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Wallet.Application.Constants;
using Wallet.Application.Interfaces.Services;
using Wallet.Application.ViewModels.Responses;

namespace Wallet.Infrastructure.Services
{
    public class WalletService : BaseService, IWalletService
    {
        private readonly ICachingService _cache;
        private readonly IAsyncRepository<Shared.Data.Models.DistributorSapAccount> _repository;
        private readonly IConfiguration _configuration;
        private readonly ISapService _Sap;
        private readonly ILogger<WalletService> _walletLogger;

        public WalletService(IAuthenticatedUserService authenticatedUserService,ICachingService cache, 
            IAsyncRepository<Shared.Data.Models.DistributorSapAccount> repository, ILogger<WalletService> walletLogger,
            IConfiguration configuration, ISapService sap) : base(authenticatedUserService)
        {
            _cache = cache;
            _repository = repository;
            _configuration = configuration;
            _Sap = sap;
            _walletLogger = walletLogger;
        }

        public async  Task<SapWalletResponse> GetMySAPWallet(int distributorSapAccountId, bool forceRefresh, CancellationToken cancellationToken)
        {
            GetUserId();
            _walletLogger.LogInformation($"{"About to retrieve Sap Wallet By The distributorSapAccountId"}{" | "}{LoggedInUser()}{" | "}{distributorSapAccountId}{" | "}{DateTime.Now}");
            SapWalletResponse response = new();

            var distributorSapAccount = _repository.Table.FirstOrDefault(p => p.UserId == _authenticatedUserService.UserId && p.DistributorSapAccountId == distributorSapAccountId);
            _walletLogger.LogInformation($"{"SAP Account Response:-"}{" | "}{JsonSerializer.Serialize(distributorSapAccount)}");


            if (distributorSapAccount == null)
            {
                response.StatusCode = ErrorCodes.Error_W_01;
                response.Status = Status.DISTRIBUTOR_SAP_ACCOUNT_NOT_FOUND;
                response.Message = ErrorMessages.DISTRIBUTOR_SAP_ACCOUNT_NOT_FOUND;
                return response;
            }


            var cacheKey = $"{CacheKeys.DMS_CUSTOMER_SAP_WALLET}{LoggedInUser}{distributorSapAccount.DistributorSapNumber}";

            var sapWallet = await _cache.GetAsync<SapWalletSapResponse>(cacheKey);
            if (sapWallet == null || forceRefresh == true)
            {
                sapWallet = _Sap.GetWallet(distributorSapAccount.CompanyCode, distributorSapAccount.CountryCode,
                distributorSapAccount.DistributorSapNumber);
                _walletLogger.LogInformation($"{"Sap Wallet Response:-"}{" | "}{JsonSerializer.Serialize(sapWallet)}");

                await _cache.SetAsync(cacheKey, sapWallet);
            }

            if (sapWallet != null)
            {
                // if (sapWallet.DistributorNumber != distributorSapAccount.DistributorSapNumber)
                if (sapWallet.DistributorNumber != sapWallet.DistributorNumber)
                {
                    response.StatusCode = ErrorCodes.Error_W_02;
                    response.Status = Status.INVALID_ROUTE;
                    response.Message = ErrorMessages.INVALID_ROUTE;
                    return response;
                }
                response.Data.AvailableBalance = sapWallet.AvailableBalance;
                response.Data.Id = sapWallet.DistributorSapAccountId;
                response.StatusCode = SuccessCodes.DEFAULT_SUCCESS_CODE;
                response.Status = Status.DEFAULT_SUCCESS;
                response.Message = SuccessMessages.SAP_WALLET_FETCHED_SUCCESSFULLY;
            }

            return await Task.FromResult(response);
        }

        public async Task<SapWalletStatementResponse> GetMySAPWalletStatement(int distributorSapAccountId, DateTime fromDate, DateTime? toDate, CancellationToken cancellationToken)
        {
            GetUserId();

            #region Logging to Logger
            _walletLogger.LogInformation($"{"About to retrieve Sap Wallet By The distributorSapAccountId"}{" | "}{LoggedInUser()}{" | "}{distributorSapAccountId}{" | "}{DateTime.Now}");
            #endregion

            SapWalletStatementResponse response = new();
            var distributorSapAccount = _repository.Table.FirstOrDefault(p => p.UserId == _authenticatedUserService.UserId && p.DistributorSapAccountId == distributorSapAccountId);

            #region Logging to Logger
            string message = $"{"SAP Account Response:-"}{" | "}{JsonSerializer.Serialize(distributorSapAccount)}";
            _walletLogger.LogInformation(message: message);
            #endregion

            if (distributorSapAccount == null)
            {
                response.StatusCode = ErrorCodes.Error_W_01;
                response.Status = Status.DISTRIBUTOR_SAP_ACCOUNT_NOT_FOUND;
                response.Message = ErrorMessages.DISTRIBUTOR_SAP_ACCOUNT_NOT_FOUND;
                return response;
            }

            var cacheKey = $"{CacheKeys.DMS_CUSTOMER_SAP_WALLET}{LoggedInUser}{distributorSapAccount.DistributorSapNumber}";

            var sapWallet = await _cache.GetAsync<SapWalletSapResponse>(cacheKey);
            if (sapWallet == null)
            {
                sapWallet =  _Sap.GetWallet(distributorSapAccount.CompanyCode, distributorSapAccount.CountryCode,
                distributorSapAccount.DistributorSapNumber);
                _walletLogger.LogInformation($"{"Sap Wallet Response:-"}{" | "}{JsonSerializer.Serialize(sapWallet)}");

                // Add the data into the cache
                await _cache.SetAsync(cacheKey, sapWallet);
            }

            var cacheKey1 = $"{CacheKeys.DMS_CUSTOMER_SAP_WALLET_ID}{LoggedInUser}{sapWallet.WalletId}";
            SapWalletSapStatementResponse sapWalletStatement = await _cache.GetAsync<SapWalletSapStatementResponse>(cacheKey1);

            DateTime calculatedToDate = toDate == null ? DateTime.Now : Convert.ToDateTime(toDate);

            if (sapWalletStatement == null || (sapWalletStatement?.FromDate > fromDate) || (sapWalletStatement?.ToDate < calculatedToDate))
            {
                List<SapWalletSapTransactionsResponse> transactions = await _Sap.GetTransactions(sapWallet.DistributorSapAccountId, fromDate, calculatedToDate);

                response = await CreateNewStatement(sapWallet.DistributorSapAccountId, sapWallet.AvailableBalance, fromDate, calculatedToDate, transactions);
                await _cache.SetAsync(cacheKey, response);
            }
            if (fromDate != null)
                response.Data.SapWalletStatement.Transactions.Where(c => c.TransactionDate >= fromDate);

            if (toDate != null)
                response.Data.SapWalletStatement.Transactions.Where(c => c.TransactionDate <= toDate);

            return await Task.FromResult(response);

        }

        #region Private Methods
        private async Task<SapWalletStatementResponse> CreateNewStatement(int distributorSapAccountId, decimal availableBalance, DateTime fromDate, DateTime calculatedToDate, List<SapWalletSapTransactionsResponse> transactions)
        {
            var response = new SapWalletStatementResponse();

            try
            {
                response.StatusCode = SuccessCodes.DEFAULT_SUCCESS_CODE;
                response.Status = Status.DEFAULT_SUCCESS;
                response.Message = SuccessMessages.SAP_WALLET_STATEMENT_FETCHED_SUCCESSFULLY;
                response.Data.SapWalletStatement = new WalletStatementReponse();
                response.Data.SapWalletStatement.AvailableBalance = availableBalance;
                response.Data.SapWalletStatement.FromDate = fromDate;
                response.Data.SapWalletStatement.ToDate = calculatedToDate;
                response.Data.SapWalletStatement.Transactions = new List<WalletTransactionResponse>();
                foreach (var transaction in transactions)
                {
                    response.Data.SapWalletStatement.Transactions.Add(
                        new WalletTransactionResponse { Amount = transaction.Amount, Description = transaction.Description, TransactionDate = transaction.TransactionDate, TransactionID = transaction.TransactionID, TransactionType = new WalletTransactionTypeResponse { Code = transaction.TransactionType?.Code, Name = transaction.TransactionType?.Name } }
                        );
                }
            }
            catch
            {
                response.StatusCode = ErrorCodes.SERVER_ERROR_CODE;
                response.Status = Status.DEFAULT_FAILURE;
                response.Message = ErrorMessages.SAP_WALLET_STATEMENT_FETCH_FAILURE;
            }

          
            return await Task.FromResult(response);
        }
        #endregion
    }
}
