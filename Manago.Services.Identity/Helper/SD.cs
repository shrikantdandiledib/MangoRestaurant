using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace Manago.Services.Identity.Helper
{
    public static class SD
    {

        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Email(),
                new IdentityResources.Profile()
            };

        public static IEnumerable<ApiScope> ApiScopes => new List<ApiScope>
        {
            new ApiScope(name:ClientScopes.Admin,displayName:"Mango Server"),
            new ApiScope(name:ClientScopes.Read,displayName:"Read your data."),
            new ApiScope(name:ClientScopes.Write,displayName:"Write your data."),
            new ApiScope(name:ClientScopes.Delete,displayName:"Delete your data.")
        };
        public static IEnumerable<Client> Clients => new List<Client>
        {
            new Client
            {
                ClientId= ClientIds.Client,
                ClientSecrets= {new Secret("secret".Sha256()) },
                AllowedGrantTypes=GrantTypes.ClientCredentials,
                AllowedScopes={ClientScopes.Read, ClientScopes.Write, ClientScopes.Delete }

            },
            new Client
            {
                ClientId=ClientIds.MangoWeb,
                ClientSecrets= {new Secret("secret".Sha256()) },
                AllowedGrantTypes=GrantTypes.Code,
                RedirectUris={ RedirectUrls .MangoWebSigninURI},
                PostLogoutRedirectUris={RedirectUrls.MangoWebSignOutURI },
                AllowedScopes=new List<string>
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.Email,
                    ClientScopes.Admin,
                }
            },

        };

    }
    public static class ClientIds
    {
        public const string Client = "client";
        public const string MangoWeb = "mango";
    }
    public static class ClientScopes
    {
        public const string Admin = "mango";
        public const string Read = "read";
        public const string Write = "write";
        public const string Delete = "delete";
    }
    public static class ClientRoles
    {
        public const string Admin = "Admin";
        public const string Customer = "Customer";
    }
    public static class RedirectUrls
    {
        public const string MangoWebSigninURI = "http://localhost:44332/signin-oidc";
        public const string MangoWebSignOutURI = "http://localhost:44332/signout-callback-oidc";
    }
}
