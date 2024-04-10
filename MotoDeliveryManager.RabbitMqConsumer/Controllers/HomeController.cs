using Microsoft.AspNetCore.Mvc;
using MotoDeliveryManager.Domain.Services.RabbitMq;
using MotoDeliveryManager.RabbitMqConsumer.Services;
using System.Reflection;
using System.Runtime.Versioning;

namespace MotoDeliveryManager.RabbitMqConsumer.Controllers
{
    public class HomeController : Controller
    {
        private readonly RabbitMqConsumerService _rabbitMqConsumerService;

        public HomeController(RabbitMqConsumerService rabbitMqConsumerService)
        {
            _rabbitMqConsumerService = rabbitMqConsumerService;
        }

        [HttpGet]
        public IActionResult Instalacao()
        {
            var informationVersion = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
            string versaocore = Assembly.GetEntryAssembly()?.GetCustomAttribute<TargetFrameworkAttribute>()?.FrameworkName;

            bool isRabbitMQConnectionActive = CheckRabbitMQConnection();

            return Ok($@"Instalação .NetCore     [{versaocore}]{Environment.NewLine}" +
                       $"URL Base do app         [{HttpContext.Request.Host.Value}]{Environment.NewLine}" +
                       $"Versão Api              [{informationVersion}]{Environment.NewLine}" +
                       $"Conexão com o RabbitMQ: [{(isRabbitMQConnectionActive ? "Ativa" : "Inativa")}]");
        }

        private bool CheckRabbitMQConnection()
        {
            try
            {
                _rabbitMqConsumerService.TestarFilaAtiva();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}