namespace Shared.ExternalServices.DTOs
{
    public class ProductDeletedMessage : IntegrationBaseMessage
    {
        public int ProductImageId { get; set; }
        public int DeletedByUserId { get; set; }
        public DateTime DateDeleted { get; set; }
    }
}
