using shared;
using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Test;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

builder.Services // duende identity server
    .AddIdentityServer(options =>
    {
        options.Events.RaiseInformationEvents = true;
        options.Events.RaiseErrorEvents = true;
        options.Events.RaiseSuccessEvents = true;
        options.Events.RaiseFailureEvents = true;
    })
    .AddInMemoryIdentityResources(IdentityConfig.IdentityResources)
    .AddInMemoryApiScopes(IdentityConfig.ApiScopes)
    .AddInMemoryClients(IdentityConfig.GetClients())
    .AddInMemoryApiResources(IdentityConfig.ApiResources)
    .AddTestUsers(IdentityConfig.GetUsers())
    .AddInMemoryCaching()
    .AddJwtBearerClientAuthentication();

var app = builder.Build();

app.MapGet("/", () => "hello world, I'm sso");
app.MapGet("/ip", async () => await IpAddress.GetIpAsync());
await app.UseConsul("auth");

app.Run();


record IdentityConfig
{
    public static IEnumerable<IdentityResource> IdentityResources => new List<IdentityResource> {
        new IdentityResources.OpenId(),
        new IdentityResources.Profile(),
        new IdentityResources.Email(),
    };
    public static IEnumerable<ApiScope> ApiScopes => new List<ApiScope> {
        new ApiScope("multillevelCacheOnK8s"),
    };
    public static IEnumerable<ApiResource> ApiResources => new List<ApiResource> {
        new ApiResource("a", "microservice a"){
            ApiSecrets = { new Secret("secret".Sha256()) },
            Scopes = { "multillevelCacheOnK8s" }
        },
        new ApiResource("b", "microservice b") {
            ApiSecrets = { new Secret("secret".Sha256()) },
            Scopes = { "multillevelCacheOnK8s" }
        },
    };
    public static IEnumerable<Client> GetClients() => new List<Client> {
        new Client
        {
            ClientId = "client",
            ClientName = "client",
            ClientSecrets = { new Secret("iENHbbVK@9tXdm7%QV!MFbn^9%d%El2*N5gSH%1c@80t32#oe4iJpJmtrD7%y2C^".Sha256()) },
            AllowedGrantTypes = GrantTypes.ClientCredentials,
            AllowAccessTokensViaBrowser = true,
            RequireConsent = false,
            AllowOfflineAccess = true,
            AllowedScopes = {
                IdentityServerConstants.StandardScopes.OpenId,
                IdentityServerConstants.StandardScopes.Profile,
                IdentityServerConstants.StandardScopes.OfflineAccess,
                "multillevelCacheOnK8s"
            },
            AccessTokenLifetime = 60 * 60 * 24 * 30, // 30 day
            RefreshTokenUsage = TokenUsage.ReUse,
            RefreshTokenExpiration = TokenExpiration.Absolute,
            RequirePkce = true
        }
    };
    public static List<TestUser> GetUsers() => new List<TestUser> {
        new TestUser {
             Username = "test1",
             Password = "123",
             Claims = new [] {
                 new Claim("id", "1")
             }
        },
        new TestUser {
             Username = "test2",
             Password = "123",
             Claims = new [] {
                 new Claim("id", "2")
             }
        },
        new TestUser {
             Username = "test3",
             Password = "123",
             Claims = new [] {
                 new Claim("id", "3")
             }
        },
    };
}
