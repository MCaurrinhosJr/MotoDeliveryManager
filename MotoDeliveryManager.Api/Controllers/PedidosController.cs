using Microsoft.AspNetCore.Mvc;
using MotoDeliveryManager.Domain.Interfaces.Services;
using MotoDeliveryManager.Domain.Models;

namespace MotoDeliveryManager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidosController : BaseController
    {
        private readonly IPedidoService _pedidoService;

        public PedidosController(IPedidoService pedidoService)
        {
            _pedidoService = pedidoService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Pedido>>> GetAllPedidos()
        {
            var pedidos = await _pedidoService.GetAllPedidosAsync();
            return Ok(pedidos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Pedido>> GetPedidoById(int id)
        {
            var pedido = await _pedidoService.GetPedidoByIdAsync(id);
            if (pedido == null)
            {
                return NotFound();
            }
            return pedido;
        }

        [HttpPost]
        public async Task<IActionResult> AddPedido(Pedido pedido)
        {
            await _pedidoService.AddPedidoAsync(pedido);
            return CreatedAtAction(nameof(GetPedidoById), new { id = pedido.Id }, pedido);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePedido(int id, Pedido pedido)
        {
            if (id != pedido.Id)
            {
                return BadRequest();
            }

            try
            {
                await _pedidoService.UpdatePedidoAsync(id, pedido);
            }
            catch (Exception)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPut("{id}/aceitarPedido/{entregadorId}")]
        public async Task<IActionResult> AceitarPedido(int id, Pedido pedido, int entregadorId)
        {
            if (id != pedido.Id)
            {
                return BadRequest();
            }

            try
            {
                await _pedidoService.AceitarPedidoAsync(id, pedido, entregadorId);
            }
            catch (Exception)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPut("{id}/entragarPedido/{entregadorId}")]
        public async Task<IActionResult> EntregarPedido(int id, Pedido pedido, int entregadorId)
        {
            if (id != pedido.Id)
            {
                return BadRequest();
            }

            try
            {
                await _pedidoService.EntregarPedidoAsync(id, pedido, entregadorId);
            }
            catch (Exception)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemovePedido(int id)
        {
            var pedido = await _pedidoService.GetPedidoByIdAsync(id);
            if (pedido == null)
            {
                return NotFound();
            }

            await _pedidoService.RemovePedidoAsync(id);

            return NoContent();
        }
    }
}
