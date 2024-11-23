using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Configuration;
using WEB_253505_Stanishewski.UI.APIConnection;
using WEB_253505_Stanishewski.UI.APIConnection.Services;
using WEB_253505_Stanishewski.UI.Data;
using WEB_253505_Stanishewski.UI.Extensions;
using WEB_253505_Stanishewski.UI.HelperClasses;
using WEB_253505_Stanishewski.UI.Services.GameService;

namespace WEB_253505_Stanishewski.UI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var uriData = builder.Configuration.GetSection("UriData").Get<UriData>();

            builder.Services.AddRazorPages();
            builder.Services.AddRazorComponents();

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            
            var keycloakData = builder.Configuration.GetSection("Keycloak").Get<KeycloakData>();

            builder.Services
                .AddAuthentication(options =>
                {
                    options.DefaultScheme =
            CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme =
            OpenIdConnectDefaults.AuthenticationScheme;
                })
                .AddCookie()
                .AddJwtBearer()
                .AddOpenIdConnect(options =>
                {
                    options.Authority =$"{keycloakData.Host}/auth/realms/{keycloakData.Realm}";
                    options.ClientId = keycloakData.ClientId;
                    options.ClientSecret = keycloakData.ClientSecret;
                    options.ResponseType = OpenIdConnectResponseType.Code;
                    options.Scope.Add("openid"); // Customize scopes as needed 
                    options.SaveTokens = true;
                    options.RequireHttpsMetadata = false;
                    options.GetClaimsFromUserInfoEndpoint = true;
                    options.ClaimActions.MapJsonKey("avatar", "avatar");
                    options.MetadataAddress =
            $"{keycloakData.Host}/realms/{keycloakData.Realm}/.well-known/openid-configuration"; 
            });

            builder.Services.AddControllersWithViews();
            builder.RegisterCustomServices(uriData);
            
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.UseWebSockets();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }
    }
}
