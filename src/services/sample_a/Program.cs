using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGet("/", () => "hello world, I'm a microservice");
app.MapGet("/ip", async () => 
    
);
app.Run();