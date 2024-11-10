using BlogWebSite.Shared;
using BlogWebSite.Shared.RenderModes;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

const bool isAutoProject = true;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

if (isAutoProject is false)
{
    builder.RootComponents.Add<Routes>("#app");
    builder.RootComponents.Add<HeadOutlet>("head::after");
}
else { }

await AddIOC(builder);

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress),
});

builder.Services.AddMasaBlazorLocal();
builder.Services.AddSingleton<IRenderMode, WasmRenderMode>();
var app = builder.Build();
await app.RunAsync();

static async Task AddIOC(WebAssemblyHostBuilder builder)
{
    var services = builder.Services;
    var baseUrl = builder.HostEnvironment.BaseAddress;

    var siteInfo = await GetOptions<SiteInfo>(baseUrl + "api/Site/GetSiteInfo");
    services.AddSingleton(siteInfo ?? new());

    services.AddHttpApi<IAppService>().ConfigureHttpApi(o =>
    {
        //o.UseLogging = Environment.IsDevelopment();
        o.HttpHost = new Uri(baseUrl);

        // o.JsonSerializeOptions -> json 序列化选项
        // o.JsonDeserializeOptions -> json 反序列化选项
        // o.KeyValueSerializeOptions -> 键值对序列化选项
        // o.XmlSerializeOptions -> xml 序列化选项
        // o.XmlDeserializeOptions -> xml 反序列化选项
        // o.GlobalFilters -> 全局过滤器集合
    });

}

static async Task<T?> GetOptions<T>(string url)
{
    using var httpclient = new HttpClient();
    var t = await httpclient.GetFromJsonAsync<T>(url);
    return t;
}
