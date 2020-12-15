using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Blazored.Modal;
using Refit;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;
using ApiRechargement.Web.Services.Interfaces;
using ApiRechargement.Web.Services;
using Blazored.Toast;
using Microsoft.Extensions.Logging;
using ApiRechargement.Web.Common;
using Serilog;
using Serilog.Core;
using Serilog.Exceptions;

namespace ApiRechargement.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");
            builder.Services.AddTransient<IAgencyService, AgencyService>();
            builder.Services.AddBlazoredModal();
            builder.Services.AddBlazoredToast();

            AddLogger(builder);

            try
            {
                await builder.Build().RunAsync();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "An exception occurred while creating the WASM host");
                throw;
            }
        }

        private static void AddLogger(WebAssemblyHostBuilder builder)
        {
            Log.Logger = new LoggerConfiguration()
               .MinimumLevel.ControlledBy(new LoggingLevelSwitch())
               .Enrich.WithProperty("BrowserInstanceId", Guid.NewGuid().ToString())
               .Enrich.WithExceptionDetails()
               .WriteTo.BrowserConsole()
               .CreateLogger();

            var unhandledExceptionSender = new UnhandledExceptionSender();
            var unhandledExceptionProvider = new UnhandledExceptionProvider(unhandledExceptionSender);
            builder.Logging.AddProvider(unhandledExceptionProvider);
            builder.Services.AddSingleton<IUnhandledExceptionSender>(unhandledExceptionSender);
        }
    }
}
