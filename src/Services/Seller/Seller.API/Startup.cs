//using EventBus.Messages.Common;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Seller.API.Data;
//using Seller.API.EventBusConsumer;
using Seller.API.Repositories;
using FluentValidation.AspNetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Messaging.AzureBus;
using Seller.API.AzureMessaging;
using Seller.API.AzureMessaging.Extentions;

namespace Seller.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //for rabitMq messaging --start
            //services.AddMassTransit(config => {
            //    config.AddConsumer<BiddingConsumer>();
            //    config.UsingRabbitMq((ctx, cfg) => {
            //        cfg.Host(Configuration["EventBusSettings:HostAddress"]);
            //        cfg.ReceiveEndpoint(EventBusConstants.ProductAddQueue,
            //            c =>
            //            {
            //                c.ConfigureConsumer<BiddingConsumer>(ctx);
            //            }
            //            );
            //    });
            //});
            //services.AddMassTransitHostedService();
            //services.AddScoped<BiddingConsumer>();
            //for rabitMq messaging --end

            services.AddAutoMapper(typeof(Startup));
            services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            });

            services.AddControllers().AddFluentValidation(fv=>
            {
                fv.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
                fv.RegisterValidatorsFromAssemblyContaining<Startup>();
            }
                );
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Seller.API", Version = "v1" });
            });

            services.AddSingleton<IProductContext, ProductContext>();
            services.AddSingleton<IProductRepository, ProductRepository>();
            //for azure ServiceBus --start
            services.AddSingleton<IMessageBus, MessageBus_Azure>();
            services.AddSingleton<IAzureServiceBusConsumer, AzureServiceBusConsumer>();
            //for azure ServiceBus --end
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                 c.SwaggerEndpoint("/swagger/v1/swagger.json", "Seller.API v1");
                c.RoutePrefix = string.Empty;
            });
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                
            }
            app.UseCors("AllowOrigin");
            app.UseRouting();

            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            //for azure ServiceBus --start
            app.UseAzureServiceBusConsumer();
            //for azure ServiceBus --end
        }
    }
}
