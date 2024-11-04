using BlogWebSite.Client;
using BlogWebSite.Components;
using BlogWebSite.Shared;
using BlogWebSite.Shared.RenderModes;

using Microsoft.Extensions.Options;

namespace BlogWebSite
{
    public class Program
    {
        public const bool UseWasm = true;

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            AddIOC(builder);

            // Add services to the container.
            var razorServer = builder.Services.AddRazorComponents();
            razorServer.AddInteractiveServerComponents();
            if (UseWasm)
            {
                razorServer.AddInteractiveWebAssemblyComponents();
            }

            builder.Services.AddMasaBlazorLocal();
            builder.Services.AddScoped<IRenderMode, ServerRenderMode>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();
            app.UseAntiforgery();

            var razorApp = app.MapRazorComponents<App>();
            razorApp
                .AddInteractiveServerRenderMode()
                .AddAdditionalAssemblies(typeof(Client._Imports).Assembly);
            if (UseWasm)
            {
                razorApp.AddInteractiveWebAssemblyRenderMode();
            }

            app.MapControllers();
            app.Run();
        }

        static void AddIOC(WebApplicationBuilder builder)
        {
            var services = builder.Services;

            services.AddControllers().AddNewtonsoftJson();

            services.Configure<SiteInfo>(builder.Configuration.GetSection(nameof(SiteInfo)));
            services.AddScoped(sp => sp.GetService<IOptions<SiteInfo>>()!.Value);
        }
    }
}
