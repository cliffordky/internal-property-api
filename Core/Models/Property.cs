using Core.Encryption;
using Newtonsoft.Json;

namespace Core.Models
{
    public record Property(Guid Id, Guid ConsumerId, Guid SubscriberId, string PhysicalAddress, string TitleDeedNumber, string ErfNumber, string Size, string PurchaseDate, string PurchasePrice, string BondHolderName, string BondAccountNumber, string BondAmount, string ISOA3CountryCode, string ISOA3CurrencyCode, string PropertyTypeCode, DateTimeOffset RecordDate, string Hash) 
        : IHasEncryptionKey
    {
        [JsonIgnore]
        public string EncryptionKey => Id.ToString();
    }
}