using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MassTransit;
using TicketProcessor.Microservice.Consumers;
using GreenPipes;

namespace TicketProcessor.Microservice
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
            var rabbitmqUri = Environment.GetEnvironmentVariable("RABBITMQ_URI");
            services.AddMassTransit(x => 
            {
                x.AddConsumer<TicketConsumer>(); // add a new consumer 
                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg => 
                {
                    cfg.UseHealthCheck(provider);
                    cfg.Host(new Uri(rabbitmqUri), h => 
                    {
                        h.Username("guest");
                        h.Password("guest"); //TODO: use appsettings
                    });
                    cfg.ReceiveEndpoint("ticketQueue", ep => // deine the recive endpoint and configure it 
                    {
                        ep.PrefetchCount = 16;
                        ep.UseMessageRetry(r => r.Interval(2, 100));
                        ep.ConfigureConsumer<TicketConsumer>(provider);
                    });
                }));
            });
            services.AddMassTransitHostedService();
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

           
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
