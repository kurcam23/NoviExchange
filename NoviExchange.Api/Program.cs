using NoviExchange.Application;
using NoviExchange.Domain.Interfaces;
using NoviExchange.EcbClient;
using NoviExchange.EcbClient.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<EcbClientOptions>(builder.Configuration.GetSection("EcbClient"));

builder.Services.AddHttpClient<IEcbProvider, EcbProvider>();

builder.Services.AddScoped<IExchangeService, ExchangeService>();

builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
