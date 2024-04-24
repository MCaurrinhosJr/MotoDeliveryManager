using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MotoDeliveryManager.Domain.Interfaces.Repositories;
using MotoDeliveryManager.Domain.Interfaces.Services;
using MotoDeliveryManager.Domain.Services;
using MotoDeliveryManager.Domain.Services.RabbitMq;
using MotoDeliveryManager.Infra.Context;
using MotoDeliveryManager.Infra.Repositories;
using MotoDeliveryManager.RabbitMqConsumer.Services;
using RabbitMQ.Client;
using System;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();

        var connectionString = Configuration.GetConnectionString("Banco");

        services.AddDbContext<MDMDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddSingleton<RabbitMqConsumerService>();

        // Configuração do Swagger
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "MotoDeliveryManager QueueConsumer", Version = "v1" });
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        // Usar o Swagger
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "MotoDeliveryManager QueueConsumer v1");
        });

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            // Configurar rota para a raiz do aplicativo
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Instalacao}/{id?}");

            endpoints.MapControllers();
        });

        var consumerService = app.ApplicationServices.GetRequiredService<RabbitMqConsumerService>();
        consumerService.StartConsuming();
    }
}