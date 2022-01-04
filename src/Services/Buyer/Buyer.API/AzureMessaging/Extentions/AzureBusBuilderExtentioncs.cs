using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Buyer.API.AzureMessaging.Extentions
{
    public static class AzureBusBuilderExtentions
    {
        public static IAzureServiceBusConsumer azureServiceBusConsumer { get; set; }
        public static IApplicationBuilder UseAzureServiceBusConsumer(this IApplicationBuilder app)
        {
            azureServiceBusConsumer = app.ApplicationServices.GetService<IAzureServiceBusConsumer>();
            var hostapplicationLive = app.ApplicationServices.GetService<IHostApplicationLifetime>();
            hostapplicationLive.ApplicationStarted.Register(OnStart);
            hostapplicationLive.ApplicationStopped.Register(OnStop);
            return app;
        }
        private static void OnStart()
        {
            azureServiceBusConsumer.Start();
        }
        private static void OnStop()
        {
            azureServiceBusConsumer.Stop();
        }
    }
}
