using Microsoft.AspNetCore.Mvc;
using MotoDeliveryManager.Domain.Interfaces.Services;
using MotoDeliveryManager.Domain.Models;

namespace MotoDeliveryManager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificacaoController : BaseController
    {
        private readonly INotificacaoService _notificacaoService;

        public NotificacaoController(INotificacaoService notificacaoService)
        {
            _notificacaoService = notificacaoService;
        }

        [HttpGet("NotificacaoPorEntregador/{entregadorId}")]
        public async Task<ActionResult<List<Notificacao>>> GetNotificacaoPorEntregadorId(int entregadorId)
        {
            try
            {
                var entregadores = await _notificacaoService.GetNotificacoesPorEntregadorAsync(entregadorId);
                return Ok(entregadores);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno ao buscar notificações por Entregador: {ex.Message}");
            }
        }

        [HttpGet("EntregadoresNotificados/{pedidoId}")]
        public async Task<ActionResult<List<Entregador>>> GetEntregadoresNotificados(int pedidoId)
        {
            try
            {
                var entregadores = await _notificacaoService.GetEntregadoresNotificadosAsync(pedidoId);
                return Ok(entregadores);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno ao buscar Entregadores Notificados: {ex.Message}");
            }
        }
    }
}