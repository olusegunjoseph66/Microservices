namespace Shared.ExternalServices.APIServices
{
    public class SapWalletSapResponse
    {
        public int WalletId { get; set; }
        public decimal AvailableBalance { get; set; }
        public int DistributorSapAccountId { get; set; }
        public string DistributorNumber { get; set; } = "";
    }
}