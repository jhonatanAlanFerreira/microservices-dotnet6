using GeekShopping.IdentityServer.Model;
using GeekShopping.IdentityServer.Model.Context;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace GeekShopping.IdentityServer.Initializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly MySQLContext _context;
        private readonly UserManager<ApplicationUser> _user;
        private readonly RoleManager<IdentityRole> _role;

        public DbInitializer(MySQLContext context, UserManager<ApplicationUser> user, RoleManager<IdentityRole> role)
        {
            _context = context;
            _user = user;
            _role = role;
        }

        public void Initialize()
        {
            if (_role.FindByNameAsync(Config.Admin).Result != null) return;

            _role.CreateAsync(new IdentityRole(Config.Admin)).GetAwaiter().GetResult();
            _role.CreateAsync(new IdentityRole(Config.Client)).GetAwaiter().GetResult();

            ApplicationUser admin = new ApplicationUser()
            {
                UserName = "jhonatan-admin",
                Email = "jhonatan@@@email",
                EmailConfirmed = true,
                PhoneNumber = "+55 (34) 12345-6789",
                FirstName = "Jhonatan",
                LastName = "Ferreira"
            };

            _user.CreateAsync(admin, "Jhonatan123$").GetAwaiter().GetResult();
            _user.AddToRoleAsync(admin, Config.Admin).GetAwaiter().GetResult();

            var adminClaims = _user.AddClaimsAsync(admin, new Claim[] {
                new Claim(JwtClaimTypes.Name, $"{admin.FirstName} {admin.LastName}"),
                new Claim(JwtClaimTypes.GivenName, $"{admin.FirstName} {admin.LastName}"),
                new Claim(JwtClaimTypes.FamilyName, $"{admin.LastName} {admin.LastName}"),
                new Claim(JwtClaimTypes.Role, Config.Admin)
            }).GetAwaiter().GetResult();

            ApplicationUser client = new ApplicationUser()
            {
                UserName = "jhonatan-client",
                Email = "jhonatan@@@email",
                EmailConfirmed = true,
                PhoneNumber = "+55 (34) 12345-6789",
                FirstName = "Jhonatan",
                LastName = "Ferreira"
            };

            _user.CreateAsync(client, "Jhonatan123$").GetAwaiter().GetResult();
            _user.AddToRoleAsync(client, Config.Client).GetAwaiter().GetResult();

            var clientClaims = _user.AddClaimsAsync(client, new Claim[] {
                new Claim(JwtClaimTypes.Name, $"{client.FirstName} {client.LastName}"),
                new Claim(JwtClaimTypes.GivenName, $"{client.FirstName} {client.LastName}"),
                new Claim(JwtClaimTypes.FamilyName, $"{client.LastName} {client.LastName}"),
                new Claim(JwtClaimTypes.Role, Config.Client)
            }).GetAwaiter().GetResult();
        }
    }
}
