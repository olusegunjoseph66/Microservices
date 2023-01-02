namespace Shared.ExternalServices.DTOs
{
    public class ProductImageUploadedMessage : IntegrationBaseMessage
    {
        public int ProductImageId { get; set; }
        public int ProductId { get; set; }
        public int CreatedByUserId { get; set; }
        public string PublicUrl { get; set; }
        public string CloudPath { get; set; }

    }
}
