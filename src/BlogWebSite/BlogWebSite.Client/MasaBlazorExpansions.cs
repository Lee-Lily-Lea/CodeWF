namespace BlogWebSite.Client;

public static class MasaBlazorExpansions
{
    public static IMasaBlazorBuilder AddMasaBlazorLocal(
        this IServiceCollection services,
        ServiceLifetime masaBlazorServiceLifetime = ServiceLifetime.Scoped
    )
    {
        return services.AddMasaBlazor(opt => { /*opt.ConfigureTheme(c=>c.CurrentTheme.)*/}, masaBlazorServiceLifetime);
    }
}
