using Microsoft.AspNetCore.Mvc;
using MotoDeliveryManager.Domain.Interfaces.Repositories;
using MotoDeliveryManager.Domain.Models;

namespace MotoDeliveryManager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MotosController : BaseController
    {
        private readonly IMotoRepository _motoRepository;

        public MotosController(IMotoRepository motoRepository)
        {
            _motoRepository = motoRepository;
        }

        // GET: api/Motos
        [HttpGet]
        public async Task<ActionResult<List<Moto>>> GetMotos()
        {
            var motos = await _motoRepository.GetAllAsync();
            return Ok(motos);
        }

        // GET: api/Motos/Placa/ABC1234
        [HttpGet("Placa/{placa}")]
        public async Task<ActionResult<List<Moto>>> GetMotoByPlaca(string placa)
        {
            var motos = await _motoRepository.GetByPlacaAsync(placa);
            return Ok(motos);
        }

        // GET: api/Motos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Moto>> GetMoto(int id)
        {
            var moto = await _motoRepository.GetByIdAsync(id);
            if (moto == null)
            {
                return NotFound();
            }
            return Ok(moto);
        }

        // POST: api/Motos
        [HttpPost]
        public async Task<ActionResult<Moto>> PostMoto(Moto moto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _motoRepository.AddAsync(moto);
            return CreatedAtAction(nameof(GetMoto), new { id = moto.Id }, moto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutMoto(int id, [FromBody] Moto moto)
        {
            if (id != moto.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingMoto = await _motoRepository.GetByIdAsync(id);
            if (existingMoto == null)
            {
                return NotFound();
            }

            existingMoto.Placa = moto.Placa; // Apenas atualiza a placa
            await _motoRepository.UpdateAsync(existingMoto);
            return NoContent();
        }

        // DELETE: api/Motos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMoto(int id)
        {
            var moto = await _motoRepository.GetByIdAsync(id);
            if (moto == null)
            {
                return NotFound();
            }

            await _motoRepository.RemoveAsync(id);
            return NoContent();
        }
    }
}
