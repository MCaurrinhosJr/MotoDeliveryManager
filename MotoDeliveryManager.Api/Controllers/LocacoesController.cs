using Microsoft.AspNetCore.Mvc;
using MotoDeliveryManager.Domain.Interfaces.Services;
using MotoDeliveryManager.Domain.Models;

namespace MotoDeliveryManager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocacoesController : BaseController
    {
        private readonly ILocacaoService _locacaoService;

        public LocacoesController(ILocacaoService locacaoService)
        {
            _locacaoService = locacaoService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Locacao>>> GetAllAsync()
        {
            return await _locacaoService.GetAllLocacoesAsync();
        }

        [HttpPost("alugar")]
        public async Task<IActionResult> AlugarMoto([FromBody] AluguelRequest request)
        {
            try
            {
                var locacao = await _locacaoService.AlugarMotoAsync(request);
                return CreatedAtAction(nameof(GetLocacaoById), new { id = locacao.Id }, locacao);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno ao alugar moto: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Locacao>> GetLocacaoById(int id)
        {
            var locacao = await _locacaoService.GetLocacaoByIdAsync(id);
            if (locacao == null)
            {
                return NotFound();
            }
            return locacao;
        }


        [HttpPut("{id}/devolucao")]
        public async Task<IActionResult> DevolverMoto(int id, [FromBody] DevolucaoRequest request)
        {
            if (id != request.LocacaoId)
            {
                return BadRequest();
            }
        
            try
            {
                await _locacaoService.DevolverMotoAsync(request);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno ao devolver moto: {ex.Message}");
            }
        
            return NoContent();
        }
    }
}