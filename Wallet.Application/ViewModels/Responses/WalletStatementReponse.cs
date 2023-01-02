namespace Wallet.Application.ViewModels.Responses
{
    public class WalletStatementReponse
    {
        public string WalletId { get; set; } = "";
        public decimal AvailableBalance { get; set; }
        public DateTime FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public List<WalletTransactionResponse> Transactions {get; set;}  
    }
}