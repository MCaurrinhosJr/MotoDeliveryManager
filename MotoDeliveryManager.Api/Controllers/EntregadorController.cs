using Microsoft.AspNetCore.Mvc;
using MotoDeliveryManager.Domain.Interfaces.Services;
using MotoDeliveryManager.Domain.Models;

namespace MotoDeliveryManager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EntregadorController : BaseController
    {
        private readonly IEntregadorService _entregadorService;

        public EntregadorController(IEntregadorService entregadorService)
        {
            _entregadorService = entregadorService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEntregadores()
        {
            try
            {
                var entregadores = await _entregadorService.GetAllEntregadoresAsync();
                return Ok(entregadores);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno ao buscar entregadores: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Entregador>> GetEntregadorById(int id)
        {
            var entregador = await _entregadorService.GetEntregadorByIdAsync(id);
            if (entregador == null)
            {
                return NotFound();
            }
            return entregador;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Entregador entregador)
        {
            try
            {
                await _entregadorService.AddEntregadorAsync(entregador);
                return Ok(entregador);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno ao adicionar entregador: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEntregador(int id, Entregador entregador)
        {
            if (id != entregador.Id)
            {
                return BadRequest();
            }

            try
            {
                await _entregadorService.UpdateEntregadorAsync(id, entregador);
            }
            catch (Exception)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveEntregador(int id)
        {
            try
            {
                await _entregadorService.RemoveEntregadorAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno ao remover entregador: {ex.Message}");
            }
        }
    }
}