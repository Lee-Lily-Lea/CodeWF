using BlogWebSite.Shared.RenderModes;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;


const bool isAutoProject = true;

if (isAutoProject)
{
    var builder = WebAssemblyHostBuilder.CreateDefault(args);

    builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

    builder.Services.AddMasaBlazorLocal();
    builder.Services.AddSingleton<IRenderMode, WasmRenderMode>();

    var siteopt = await GetOptions(builder.HostEnvironment.BaseAddress);
    builder.Services.AddSingleton(siteopt);

    var app = builder.Build();
    await app.RunAsync();
}
else
{
    var builder = WebAssemblyHostBuilder.CreateDefault(args);
    builder.RootComponents.Add<Routes>("#app");
    builder.RootComponents.Add<HeadOutlet>("head::after");

    builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });


    builder.Services.AddMasaBlazorLocal();
    builder.Services.AddSingleton<IRenderMode, WasmRenderMode>();

    await builder.Build().RunAsync();
}


async Task<SiteOption> GetOptions(string url)
{
    using (var httpclient = new HttpClient() { BaseAddress = new Uri(url) })
    {
        return await httpclient.GetFromJsonAsync<SiteOption>("files/json/SiteOption.json");
    };
}
