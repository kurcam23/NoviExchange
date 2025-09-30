using Microsoft.EntityFrameworkCore;
using NoviExchange.Application;
using NoviExchange.Application.Factories;
using NoviExchange.Application.Interfaces.Factories;
using NoviExchange.Application.Interfaces.Providers;
using NoviExchange.Application.Interfaces.Repositories;
using NoviExchange.Application.Interfaces.Services;
using NoviExchange.Application.Jobs;
using NoviExchange.Application.Profiles;
using NoviExchange.EcbClient;
using NoviExchange.EcbClient.Options;
using NoviExchange.Infrastructure;
using NoviExchange.Infrastructure.Repositories;
using Quartz;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<NoviExchangeDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.Configure<EcbClientOptions>(builder.Configuration.GetSection("EcbClient"));

builder.Services.AddHttpClient<IEcbProvider, EcbProvider>();

builder.Services.AddScoped<IWalletAdjustmentStrategyFactory, WalletAdjustmentStrategyFactory>();

builder.Services.AddScoped<IExchangeService, ExchangeService>();
builder.Services.AddScoped<IWalletService, WalletService>();

builder.Services.AddScoped<ICurrencyRateRepository, CurrencyRateRepository>();
builder.Services.AddScoped<IWalletRepository, WalletRepository>();

builder.Services.AddControllers();

builder.Services.AddAutoMapper(typeof(WalletProfile));

builder.Services.AddQuartz(q =>
{
    q.SchedulerId = "Scheduler-Core";

    var jobKey = new JobKey("ExchangeRateUpdateJob");

    q.AddJob<ExchangeRateUpdateJob>(opts => opts.WithIdentity(jobKey));

    q.AddTrigger(opts => opts
        .ForJob(jobKey)
        .WithIdentity("ExchangeRateUpdateTrigger")
        .WithSimpleSchedule(x => x
            .WithInterval(TimeSpan.FromMinutes(1))
            .RepeatForever()));
});

builder.Services.AddQuartzHostedService(options => { options.WaitForJobsToComplete = true; });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
