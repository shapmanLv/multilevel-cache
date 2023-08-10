using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using Ocelot.Provider.Polly;

var builder = WebApplication.CreateBuilder(args);

builder.Services
  .AddOcelot()
  .AddConsul()
  .AddPolly();

var app = builder.Build();

await app.UseOcelot();

app.Run();
