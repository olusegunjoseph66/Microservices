namespace Wallet.Application.ViewModels.Responses
{
    public class WalletTransactionResponse
    {
        public string TransactionID { get; set; } = "";

        public decimal Amount { get; set; }

        public string Description { get; set; } = "";

        public DateTime TransactionDate { get; set; }

        public WalletTransactionTypeResponse TransactionType { get; set; } =new WalletTransactionTypeResponse();
    }
}