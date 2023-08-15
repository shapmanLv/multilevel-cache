using Consul;
using Microsoft.AspNetCore.Builder;

namespace shared;
public static class CnosulExtension
{
    public static async Task UseConsul(this WebApplication app, string serviceName)
    {
        var consulClient = new ConsulClient(config => config.Address = new Uri("http://consul:8500"));
        var ip = await IpAddress.GetIpAsync();
        var serviceId = Guid.NewGuid().ToString("N");
        app.MapGet("/health", () => "I am fit");
        await consulClient.Agent.ServiceRegister(new AgentServiceRegistration
        {
            ID = serviceId,
            Address = ip,
            Name = serviceName,
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
    }
}
