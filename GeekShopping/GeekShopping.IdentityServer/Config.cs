using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace GeekShopping.IdentityServer;

public static class Config
{
    public const string Admin = "Admin";
    public const string Client = "Client";
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Email(),
            new IdentityResources.Profile()
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
            {
                new ApiScope("geek_shopping", "GeekShopping Server"),
                new ApiScope("read", "Read Data"),
                new ApiScope("write", "Write Data"),
                new ApiScope("delete", "Delete Data"),
            };

    public static IEnumerable<Client> Clients =>
        new Client[]
            {
                new Client
                {
                    ClientId = "client",
                    ClientSecrets = {new Secret("my_super_secret".Sha256())},
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = {"read", "write", "profile"}
                },
                new Client
                {
                    ClientId = "geek_shopping",
                    ClientSecrets = {new Secret("my_super_secret".Sha256())},
                    AllowedGrantTypes = GrantTypes.Code,
                    RedirectUris = {"https://localhost:4430/signin-oidc"},
                    PostLogoutRedirectUris = {"https://localhost:4430/signout-callback-oidc"},
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "geek_shopping"
                    }
                }
            };
}