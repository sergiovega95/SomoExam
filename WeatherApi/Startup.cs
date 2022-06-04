using Azure.Data.Tables;
using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using WeatherApiDomain.Implementations.Weather;
using WeatherApiDomain.Interfaces.ExternalServices;
using WeatherApiDomain.Interfaces.Weather;
using WeatherApiInfraestructure.Implementations;
using System.Reflection;

namespace WebApplication1
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;          

            var config = new ConfigurationBuilder()
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json",true)
            .Build();                     
      
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {           
            services.AddHttpClient();
            services.AddControllers();
            services.AddScoped<IConsumerServices, ConsumerServices>();
            services.AddScoped<IWeatherInfo,WeatherInfo>();
            services.AddSingleton((s) =>
            {
                return new TableServiceClient(Configuration["ApplicationConfig:AzureTableStorage:StorageConnectionString"]);
            });
            services.AddScoped<IStorage, AzureTableStorage>();
            services.AddSingleton((s) => 
            {
                return new ServiceBusClient(Configuration["ApplicationConfig:AzureServiceBus:ConnectionString"], new ServiceBusClientOptions() { TransportType = ServiceBusTransportType.AmqpWebSockets });
            });
            services.AddScoped<IMessageBroker, AzureServiceBus>();                       

            services.AddSwaggerGen(options =>
            {
                var groupName = "v1";
                options.SwaggerDoc(groupName, new OpenApiInfo
                {
                    Title = $"Somo Exam Weather Api",
                    Version = groupName,
                    Description = "Weather api that return current weather of a city, and storage retrived data on external storage service",
                    Contact = new OpenApiContact
                    {
                        Name = "Sergio andres vega vasquez",
                        Email = "Sergiovega9511@gmail.com"                       
                    }
                    
                });               
                
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Somo Exam Weather Api");
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
