using shared;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "hello world, I'm b microservice");
app.MapGet("/ip", async () => await IpAddress.GetIpAsync());
await app.UseConsul("sample_b");

app.Run();

