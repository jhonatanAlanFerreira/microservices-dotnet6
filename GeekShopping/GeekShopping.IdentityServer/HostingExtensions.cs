using Duende.IdentityServer.Services;
using GeekShopping.IdentityServer.Initializer;
using GeekShopping.IdentityServer.Model;
using GeekShopping.IdentityServer.Model.Context;
using GeekShopping.IdentityServer.Services;
using Microsoft.AspNetCore.Identity;
using Serilog;

namespace GeekShopping.IdentityServer;

internal static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {

        builder.Services.AddRazorPages();

        builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<MySQLContext>().AddDefaultTokenProviders();

        builder.Services.AddIdentityServer(options =>
        {
            options.Events.RaiseErrorEvents = true;
            options.Events.RaiseInformationEvents = true;
            options.Events.RaiseFailureEvents = true;
            options.Events.RaiseSuccessEvents = true;
            options.EmitStaticAudienceClaim = true;
        }).AddInMemoryIdentityResources(Config.IdentityResources)
        .AddInMemoryApiScopes(Config.ApiScopes)
        .AddInMemoryClients(Config.Clients)
        .AddAspNetIdentity<ApplicationUser>()
        .AddDeveloperSigningCredential();

        builder.Services.AddScoped<IDbInitializer, DbInitializer>();
        builder.Services.AddScoped<IProfileService, ProfileService>();

        return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        app.UseSerilogRequestLogging();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();

        app.UseIdentityServer();

        app.UseAuthorization();

        app.MapRazorPages().RequireAuthorization();

        app.Services.CreateScope().ServiceProvider.GetService<IDbInitializer>().Initialize();

        return app;
    }
}
