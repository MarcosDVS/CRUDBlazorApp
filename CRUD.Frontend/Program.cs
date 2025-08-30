using CRUD.Frontend.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace CRUD.Frontend;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");

        #region HttpClient del api -- Permitir conexiones desde el backend
        var url = "https://localhost:7002"; // Adjust the URL to match your backend API endpoint
        builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(url) });
        #endregion

        #region Inyeccion de dependencias de servicios
        builder.Services.AddScoped<IRepository, Repository>();
        builder.Services.AddScoped<IAutoServices, AutoServices>();
        #endregion

        await builder.Build().RunAsync();
    }
}
