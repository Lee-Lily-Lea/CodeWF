using BlogWebSite.Client;
using BlogWebSite.Components;
using BlogWebSite.Services;
using BlogWebSite.Services.Configure;
using BlogWebSite.Shared;
using BlogWebSite.Shared.RenderModes;

using Masa.Blazor;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

namespace BlogWebSite
{
    public class Program
    {
        public const bool UseWasm = false;

        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var razorServer = builder.Services.AddRazorComponents();
            razorServer.AddInteractiveServerComponents();
            if (UseWasm)
            {
                razorServer.AddInteractiveWebAssemblyComponents();
            }

            builder.Services.AddMasaBlazorLocal();
            builder.Services.AddScoped<IRenderMode, ServerRenderMode>();


            await AddIOC(builder);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseWebAssemblyDebugging();
                app.UseSwagger();
                app.UseSwaggerUI(opt => opt.RoutePrefix = "Swagger");
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

        static async Task AddIOC(WebApplicationBuilder builder)
        {
            var services = builder.Services;

            services.AddControllers().AddNewtonsoftJson();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });

            var infoConfig = builder.Configuration.GetSection(nameof(SiteInfo));
            services.Configure<SiteInfo>(infoConfig);

            services.AddScoped(sp => sp.GetService<IOptions<SiteInfo>>()!.Value);

            var opt = infoConfig.Get<SiteOption>();
            //services.AddSingleton<SiteOption>(opt);

            var appService = new AppService(opt);
            await appService.SeedAsync();
            //services.AddSingleton<AppService>(appService);
            services.AddSingleton<IAppService>(appService);
        }


    }
}
