using common;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "hello world, I'm b microservice");
app.MapGet("/ip", async () => await IpAddress.GetIpAsync());
app.MapGet("/health", () => "I am fit");

app.Run();

