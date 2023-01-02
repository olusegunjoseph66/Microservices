namespace Shared.ExternalServices.DTOs
{
    public class ProductUpdatedMessage : IntegrationBaseMessage
    {
        public int ProductStatusId { get; set; }
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string UnitOfMeasure { get; set; }
        public string SapId { get; set; }
    }
}
