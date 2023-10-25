using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.OpenApi.Models;
using BankStatementApi.BLL.Infrastructure;
using BankStatementApi.DAL.Repositories;
using BankStatementApi.DAL.EF;
using BankStatementApi.DAL.Interfaces;
using BankStatementApi.BLL.Services;
using BankStatementApi.BLL.BusinessLogic.Event;
using MediatR;
using BankStatementApi.BLL.BusinessLogic.Event.Consumer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using BankStatementApi.BLL.Services.Interfaces;
using BankStatementApi.BLL.BusinessLogic.Event.Model;
using RMQCommonCore.Interfaces;
using RMQCommonCore;
using Ibzkh_SecurityNET6.IdentityProvider;
using RMQCommonCore.IntegrationEventLogEF.Configuration;
using NLog.Web;

var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

try
{
    var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
    var building = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .AddJsonFile($"appsettings.{environment}.json", optional: true)
        .AddEnvironmentVariables();
    IConfigurationRoot configuration = building.Build();
    Console.WriteLine(environment);
    var builder = WebApplication.CreateBuilder(args);

    #region Logging
    builder.Logging.ClearProviders();
    builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Warning);
    builder.Host.UseNLog();
    #endregion

    #region ConfigureServices
    var connection = builder.Configuration.GetConnectionString("DefaultConnection");
    builder.Services.AddDbContext<BankStatementContext>(options => options.UseNpgsql(connection));
    builder.Services.Configure<KestrelServerOptions>(builder.Configuration.GetSection("Kestrel"));
    builder.Services.AddControllers();
    builder.Services.AddAutoMapper(typeof(MappingProfile));
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddHttpContextAccessor();
    builder.Services.AddCors();

    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "v1",
            Title = "BankStatementApi",
            Description = "API for interaction with the client bank",
            //TermsOfService = new Uri("https://example.com/terms"),
            //Contact = new OpenApiContact
            //{
            //    Name = "Example Contact",
            //    Url = new Uri("https://example.com/contact")
            //},
            //License = new OpenApiLicense
            //{
            //    Name = "Example License",
            //    Url = new Uri("https://example.com/license")
            //}
        });
    });

    #region Secure
    builder.Services.AddTransient<IIdentityProvider, IdentityProviderCore>();
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
         {
             options.Authority = "https://signinoidc.demo.ibzkh.ru/";
             options.TokenValidationParameters = new TokenValidationParameters
             {
                 ValidateAudience = false,
             };
         });
    #endregion

    ////добавляет политику авторизации, чтобы убедиться, что токен предназначен для области "api"
    //builder.Services.AddAuthorization(options =>
    //{
    //    options.AddPolicy("ApiScope_Overhaul" , policy =>
    //     {
    //         policy.RequireAuthenticatedUser();
    //         policy.RequireClaim("scope" , "overhaul.api");
    //     });
    //});

    RegistryRabbitMq(builder);
    builder.Services.AddSingleton<DictionaryStorage>();
    builder.Services.AddScoped<IBankStatementUnitOfWork, BankStatementUnitOfWork>(); 
    builder.Services.AddScoped<IServiceInfrastructure, ServiceInfrastructure>();

    builder.Services.AddTransient<IDictionaryServices, DictionaryServices>();
    builder.Services.AddTransient<IOperationNumberGenerationServices, OperationNumberGenerationServices>();
    builder.Services.AddTransient<IBankStatementServices, BankStatementServices>();
    #endregion

    #region Configure
    var app = builder.Build();

    //if (app.Environment.IsDevelopment())
    //{
    app.UseSwagger();
    app.UseSwaggerUI();
    //(options =>
    //{
    //  options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    //  options.RoutePrefix = string.Empty;
    //});
    //}

    app.UseDefaultFiles();
    app.UseStaticFiles();
    //app.UseHttpsRedirection();
    app.UseRouting();

    app.UseCors(builder => builder.WithOrigins(app.Configuration.GetSection("Cors")
                .GetChildren().Select(m => m.Value).ToArray())
            .AllowAnyMethod()
            .AllowAnyHeader());
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();
    app.Run();
    #endregion
}
catch (Exception exception)
{
    logger.Error(exception, "Stopped program because of exception");
    throw;
}
finally
{
    NLog.LogManager.Shutdown();
}

#region RabbitMQ
static void RegistryRabbitMq(WebApplicationBuilder builder)
{
    builder.RegistryServiceBus();
    RegistryProducers(builder);
    RegistryConsumers(builder);
}

static void RegistryProducers(WebApplicationBuilder builder)
{
    builder.Services.AddSingleton<IRabbitMqProducer<DataLogEvent>, GeneralProducerEvent<DataLogEvent>>();
    builder.Services.AddSingleton<IRabbitMqProducer<PaymentOrderEvent>, GeneralProducerEvent<PaymentOrderEvent>>();
    builder.Services.AddSingleton<IProducerUnitOfWork, ProducerUnitOfWork>();
}

static void RegistryConsumers(WebApplicationBuilder builder)
{
    builder.Services.AddSingleton<IRequestHandler<DataLogEvent, Unit>, ReceivedDataLogEventHandler>();
    builder.Services.AddHostedService<GeneralConsumerEvent<DataLogEvent>>();
}
#endregion