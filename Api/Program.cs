
using Coravel;
using Core.Encryption;
using Core.Models;
using Marten;
using Serilog;
using Weasel.Core;

namespace Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            const string key = "tKDM8/ZCTZkRtKi7ZKDALBTEE/+WmMA5SEpWp02Y0qs=";
            const string iv = "L/G6cEvpCK/0XUS2kWsKoA==";

            // Create an instance of the encryption service
            var encryptionService = new AesEncryptionService(key, iv);

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.IncludeXmlComments(string.Format(@"{0}\Api.xml", System.AppDomain.CurrentDomain.BaseDirectory));
                //c.SwaggerDoc("v1", new OpenApiInfo
                //{
                //    Version = "v1",
                //    Title = "Api"
                //});
            });

            builder.Services.AddCache();
            // This is the absolute, simplest way to integrate Marten into your
            // .NET application with Marten's default configuration
            builder.Services.AddMarten(options =>
            {
                // Establish the connection string to your Marten database
                options.Connection(builder.Configuration.GetConnectionString("db_connection")!);

                // Specify that we want to use STJ as our serializer
                //options.UseSystemTextJsonForSerialization();
                options.UseEncryptionRulesForProtectedInformation(encryptionService);

                options.Schema.For<Property>()
                    .AddEncryptionRuleForProtectedInformation(x => x.PhysicalAddress)
                    .AddEncryptionRuleForProtectedInformation(x => x.TitleDeedNumber)
                    .AddEncryptionRuleForProtectedInformation(x => x.ErfNumber)
                    .AddEncryptionRuleForProtectedInformation(x => x.Size)
                    .AddEncryptionRuleForProtectedInformation(x => x.PurchaseDate)
                    .AddEncryptionRuleForProtectedInformation(x => x.PurchasePrice)
                    .AddEncryptionRuleForProtectedInformation(x => x.BondHolderName)
                    .AddEncryptionRuleForProtectedInformation(x => x.BondAccountNumber)
                    .AddEncryptionRuleForProtectedInformation(x => x.BondAmount)
                    .AddEncryptionRuleForProtectedInformation(x => x.PropertyTypeId)
                    .AddEncryptionRuleForProtectedInformation(x => x.PurchaseDate);
                    //.AddEncryptionRuleForProtectedInformation(x => x.ISOA3CurrencyCode)
                    //.AddEncryptionRuleForProtectedInformation(x => x.ISOA3CountryCode);
                // If we're running in development mode, let Marten just take care
                // of all necessary schema building and patching behind the scenes
                if (builder.Environment.IsDevelopment())
                {
                    options.AutoCreateSchemaObjects = AutoCreate.All;
                }
            });

            builder.Host.UseSerilog((hostingContext, loggerConfig) =>
            {
                loggerConfig.ReadFrom.Configuration(hostingContext.Configuration);
                loggerConfig.Enrich.FromLogContext();
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "internal-property-api.Api");
                });
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();
            app.UseSerilogRequestLogging();
            app.Run();
        }
    }
}
