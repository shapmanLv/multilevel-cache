using common;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "hello world, I'm a microservice");
app.MapGet("/ip", async () => await IpAddress.GetIpAsync());

app.Run();