using BankWebApi.Domain.AccountModel;
using BankWebApi.Domain.AccountModel.Commands;
using BankWebApi.Domain.AccountModel.Events;
using BankWebApi.Domain.AccountModel.Queries;
using BankWebApi.Domain.ReadModels;
using BankWebApi.Domain.TransactionModel;
using BankWebApi.Domain.TransactionModel.Queries;
using BankWebApi.Services;
using BankWebApi.Services.Dto;
using EventFlow;
using EventFlow.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddEventFlow(ef => ef
                //.AddEvents(typeof(AccountCreatedEvent))
                //.AddCommands(typeof(AccountCreateCommand))
                //.AddCommandHandlers(typeof(AccountCreateCommandHandler))
                .AddDefaults(typeof(Program).Assembly)
                .UseInMemoryReadStoreFor<AccountReadModel>()
                .UseInMemoryReadStoreFor<TransactionReadModel>()
                .AddQueryHandlers(new[] { 
                        typeof(GetAccountQueryHandler),
                        typeof(GetTransactionHistoryQueryHandler)
                })
//.AddQueryHandler<GetBalanceQueryHandler, GetBalanceQuery, Account>()               
//.AddQueryHandler<GetTransactionHistoryQueryHandler, GetTransactionHistoryQuery, Transaction>()

);

builder.Services.AddTransient<ICommandService, CommandService>();
builder.Services.AddTransient<IQueryService, QueryService>();
builder.Services.AddHttpClient();


builder.Services.AddTransient<WebhookService>();
builder.Services.Configure<WebhookOptions>(builder.Configuration.GetSection(nameof(WebhookOptions)));


Log.Logger = new LoggerConfiguration()
                          .Enrich.FromLogContext()
                          .MinimumLevel.Debug()
                          .WriteTo.Console()
                          .CreateLogger();
builder.Services.AddLogging(lb => lb.AddSerilog(Log.Logger, true));

Log.Information("No one listens to me!");


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
