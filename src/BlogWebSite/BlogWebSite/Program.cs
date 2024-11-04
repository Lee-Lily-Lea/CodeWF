using BlogWebSite.Client;
using BlogWebSite.Components;
using BlogWebSite.Shared;
using BlogWebSite.Shared.RenderModes;

namespace BlogWebSite
{
    public class Program
    {
        public const bool UseWasm = false;

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            OldIOC(builder);

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
            razorApp.AddInteractiveServerRenderMode().AddAdditionalAssemblies(typeof(Client._Imports).Assembly);
            if (UseWasm)
            {
                razorApp.AddInteractiveWebAssemblyRenderMode();
            }

            app.Run();
        }

        static void OldIOC(WebApplicationBuilder builder)
        {
            builder.Services.Configure<SiteOption>(builder.Configuration.GetSection("Site"));
            //builder.Services.AddSingleton<AppService>();
            //builder.AddApp();
        }
    }
}
