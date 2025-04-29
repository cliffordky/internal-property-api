using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using Coravel.Cache.Interfaces;
using Marten;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class DataController : ControllerBase
    {
        private readonly ILogger<DataController> _logger;
        private readonly IDocumentStore _store;
        private readonly ICache _cache;

        public DataController(ILogger<DataController> logger, IDocumentStore store, ICache cache)
        {
            _logger = logger;
            _store = store;
            _cache = cache;
        }

        //[Authorize("write")]
        [Authorize]
        [TranslateResultToActionResult]
        [HttpPost("property")]
        public async Task<Result<Models.v1.PropertyResponse>> AddProperty(Models.v1.PropertyRequest request)
        {
            try
            {
                string hash = Core.Encryption.Hash.GetHashString(request.ConsumerId.ToString() +
                    request.SubscriberId.ToString() +
                    request.PhysicalAddress +
                    request.TitleDeedNumber +
                    request.ErfNumber +
                    request.Size +
                    request.PurchaseDate.ToString() +
                    request.PurchasePrice.ToString() +
                    request.BondHolderName.ToString() +
                    request.BondAccountNumber +
                    request.BondAmount.ToString() +
                    request.PropertyTypeCode.ToString() +
                    request.ISOA3CountryCode +
                    request.ISOA3CurrencyCode +
                    request.RecordDate);

                await using var session = _store.LightweightSession();
                var existing = await session.Query<Core.Models.Property>().SingleOrDefaultAsync(x => x.Hash == hash);
                if (existing != null)
                {
                    return Result<Models.v1.PropertyResponse>.Error("Property already exists");
                }

                var property = new Core.Models.Property(
                        Guid.NewGuid(),
                        request.ConsumerId,
                        request.SubscriberId,
                        request.PhysicalAddress,
                        request.TitleDeedNumber,
                        request.ErfNumber,
                        request.Size,
                        request.PurchaseDate.ToString(),
                        request.PurchasePrice.ToString(),
                        request.BondHolderName.ToString(),
                        request.BondAccountNumber,
                        request.BondAmount.ToString(),
                        request.ISOA3CountryCode,
                        request.ISOA3CurrencyCode,
                        request.PropertyTypeCode,                     
                        request.RecordDate,
                        hash
                    );

                session.Store(property);
                await session.SaveChangesAsync();

                return Result<Models.v1.PropertyResponse>.Success(new Models.v1.PropertyResponse
                {
                    Id = property.Id,
                    ConsumerId = property.ConsumerId,
                    SubscriberId = property.SubscriberId,
                    PhysicalAddress = property.PhysicalAddress,
                    TitleDeedNumber = property.TitleDeedNumber,
                    ErfNumber = property.ErfNumber,
                    Size = property.Size,
                    PurchaseDate = DateOnly.Parse(property.PurchaseDate),
                    PurchasePrice = Decimal.Parse(property.PurchasePrice),
                    BondHolderName = property.BondHolderName,
                    BondAccountNumber = property.BondAccountNumber,
                    BondAmount = Decimal.Parse(property.BondAmount),
                    PropertyTypeCode = property.PropertyTypeCode,
                    ISOA3CountryCode = property.ISOA3CountryCode,
                    ISOA3CurrencyCode = property.ISOA3CurrencyCode,
                    RecordDate = property.RecordDate
                });
            }
            catch (Exception Ex)
            {
                return Result<Models.v1.PropertyResponse>.Error(Ex.Message);
            }
        }

        //[Authorize("read")]
        [Authorize]
        [TranslateResultToActionResult]
        [HttpGet("properties")]
        public async Task<Result<List<Models.v1.PropertyResponse>>> GetPropertiesForConsumer(Guid ConsumerId)
        {
            try
            {
                await using var session = _store.LightweightSession();
                var properties = await session.Query<Core.Models.Property>().Where(x => x.ConsumerId == ConsumerId).ToListAsync();

                return Result<List<Models.v1.PropertyResponse>>.Success(properties.Select(
                    x => new Models.v1.PropertyResponse
                    {
                        Id = x.Id,
                        ConsumerId = x.ConsumerId,
                        SubscriberId = x.SubscriberId,
                        PhysicalAddress = x.PhysicalAddress,
                        TitleDeedNumber = x.TitleDeedNumber,
                        ErfNumber = x.ErfNumber,
                        Size = x.Size,
                        PurchaseDate = DateOnly.Parse(x.PurchaseDate),
                        PurchasePrice = Decimal.Parse(x.PurchasePrice),
                        BondHolderName = x.BondHolderName,
                        BondAccountNumber = x.BondAccountNumber,
                        BondAmount = Decimal.Parse(x.BondAmount),
                        PropertyTypeCode = x.PropertyTypeCode,
                        ISOA3CountryCode = x.ISOA3CountryCode,
                        ISOA3CurrencyCode = x.ISOA3CurrencyCode,
                        RecordDate = x.RecordDate
                    }).ToList());
            }
            catch (Exception Ex)
            {
                return Result<List<Models.v1.PropertyResponse>>.Error(Ex.Message);
            }
        }
    }
}