using common;
using Consul;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
var ip = await IpAddress.GetIpAsync();

app.MapGet("/", () => "hello world, I'm a microservice");
app.MapGet("/ip", () => ip);
app.MapGet("/health", () => "I am fit");

var consulClient = new ConsulClient(config => config.Address = new Uri("http://consul:8500"));
var serviceId = Guid.NewGuid().ToString("N");
await consulClient.Agent.ServiceRegister(new AgentServiceRegistration
{
    ID = serviceId,
    Address = ip,
    Name = "sample_a",
    Port = 80,
    Check = new AgentServiceCheck
    {
        Interval = TimeSpan.FromSeconds(10),
        Timeout = TimeSpan.FromSeconds(5),
        HTTP = $"http://{ip}/health",
        DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(3),
    }
});
app.Lifetime.ApplicationStopping.Register(() =>
  consulClient.Agent.ServiceDeregister(serviceId));

app.Run();
