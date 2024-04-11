using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using MotoDeliveryManager.Infra.Context;
using Microsoft.OpenApi.Models;
using MotoDeliveryManager.Domain.Interfaces.Repositories;
using MotoDeliveryManager.Domain.Interfaces.Services;
using MotoDeliveryManager.Domain.Services;
using MotoDeliveryManager.Infra.Repositories;
using MotoDeliveryManager.Domain.Services.RabbitMq;


namespace MotoDeliveryManager.Api
{
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

            using (var dbContext = new MDMDbContext(new DbContextOptionsBuilder<MDMDbContext>().UseNpgsql(connectionString).Options))
            {
                dbContext.Database.EnsureCreated();
            }

            services.AddDbContext<MDMDbContext>(options =>
                options.UseNpgsql(connectionString));

            services.AddSingleton<RabbitMQService>();
            services.AddScoped<IRabbitMQService, RabbitMQService>();
            services.AddScoped<IEntregadorService, EntregadorService>();
            services.AddScoped<IEntregadorRepository, EntregadorRepository>();
            services.AddScoped<ILocacaoService, LocacaoService>();
            services.AddScoped<ILocacaoRepository, LocacaoRepository>();
            services.AddScoped<IMotoService, MotoService>();
            services.AddScoped<IMotoRepository, MotoRepository>();
            services.AddScoped<INotificacaoService, NotificacaoService>();
            services.AddScoped<INotificacaoRepository, NotificacaoRepository>();
            services.AddScoped<IPedidoService, PedidoService>();
            services.AddScoped<IPedidoRepository, PedidoRepository>();

            // Adicionar Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "MotoDeliveryManager API",
                    Version = "v1",
                    Description = "Lista de Endpoints da Api"
                });
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
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "MotoDeliveryManager API v1");
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
        }
    }
}