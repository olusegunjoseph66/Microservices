namespace Wallet.Application.ViewModels.Responses
{
    public class SapWalletDataVM
    {
        public SapWalletResponseItem sapWallet { get; set; }
    }
    public class SapWalletResponseItem
    {

        public int Id { get; set; }

        public decimal AvailableBalance { get; set; }
    }
}