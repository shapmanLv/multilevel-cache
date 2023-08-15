using shared;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "hello world, I'm a microservice");
app.MapGet("/ip", async () => await IpAddress.GetIpAsync());
await app.UseConsul("sample_a");

app.Run();
