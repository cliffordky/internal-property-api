namespace Api.Models.v1
{
    public class PropertyResponse
    {
        public Guid Id { get; set; }
        public Guid ConsumerId { get; set; }
        public Guid SubscriberId { get; set; }
        public string PhysicalAddress { get; set; }
        public string TitleDeedNumber { get; set; }
        public string ErfNumber { get; set; }
        public string Size { get; set; }
        public DateOnly PurchaseDate { get; set; }
        public decimal PurchasePrice { get; set; }
        public string BondHolderName { get; set; }
        public string BondAccountNumber { get; set; }
        public decimal BondAmount { get; set; }
        public string PropertyTypeCode { get; set; }
        public string ISOA3CountryCode { get; set; }
        public string ISOA3CurrencyCode { get; set; }
        public DateTimeOffset RecordDate { get; set; }
    }
}