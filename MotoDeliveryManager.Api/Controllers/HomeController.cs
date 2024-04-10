using Microsoft.AspNetCore.Mvc;
using MotoDeliveryManager.Domain.Services.RabbitMq;
using MotoDeliveryManager.Infra.Context;
using System.Reflection;
using System.Runtime.Versioning;

namespace MotoDeliveryManager.Api.Controllers
{
    public class HomeController : BaseController
    {
        private readonly MDMDbContext _dbContext;
        private readonly RabbitMQService _rabbitMQService;

        public HomeController(MDMDbContext dbContext, RabbitMQService rabbitMQService)
        {
            _dbContext = dbContext;
            _rabbitMQService = rabbitMQService;
        }

        [HttpGet]
        public IActionResult Instalacao()
        {
            var informationVersion = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
            string versaocore = Assembly.GetEntryAssembly()?.GetCustomAttribute<TargetFrameworkAttribute>()?.FrameworkName;

            bool isDbConnectionActive = CheckDbConnection();
            bool isRabbitMQConnectionActive = CheckRabbitMQConnection();

            return Ok($@"Instalação .NetCore [{versaocore}]{Environment.NewLine}" +
                       $"URL Base do app     [{HttpContext.Request.Host.Value}]{Environment.NewLine}" +
                       $"Versão Api          [{informationVersion}]{Environment.NewLine}" +
                       $"Conexão com o banco de dados: {(isDbConnectionActive ? "Ativa" : "Inativa")}{Environment.NewLine}" +
                       $"Conexão com o RabbitMQ: {(isRabbitMQConnectionActive ? "Ativa" : "Inativa")}");
        }

        private bool CheckDbConnection()
        {
            try
            {
                _dbContext.Database.CanConnect();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool CheckRabbitMQConnection()
        {
            try
            {
                _rabbitMQService.TestarFilaAtiva();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}