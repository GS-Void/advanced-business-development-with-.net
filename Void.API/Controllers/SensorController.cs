using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using Void.API.Data;
using Void.API.DTOs;
using Void.API.Models;

namespace Void.API.Controllers
{
    [Route("api/sensor")]
    [ApiController]
    [Authorize(Roles = "Fisioterapeuta")] // Apenas profissionais gerenciam hardware
    public class SensorController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public SensorController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Listar todos os sensores cadastrados")]
        public async Task<IActionResult> Get()
        {
            var sensores = await _context.Sensores.ToListAsync();
            if (!sensores.Any()) return NoContent();

            return Ok(sensores.Select(s => new SensorResponseDTO
            { Id = s.Id, MacAddress = s.MacAddress, Status = s.Status }));
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Cadastrar novo dispositivo ESP32")]
        public async Task<IActionResult> Post([FromBody] SensorRequestDTO model)
        {
            var sensor = new SensorWearableEntity
            {
                MacAddress = model.MacAddress,
                Status = model.Status
            };

            _context.Sensores.Add(sensor);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = sensor.Id }, model);
        }

        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Atualizar status do sensor (ex: ATIVO para MANUTENCAO)")]
        public async Task<IActionResult> Put(int id, [FromBody] SensorRequestDTO model)
        {
            var sensor = await _context.Sensores.FirstOrDefaultAsync(x => x.Id == id);
            if (sensor == null) return NotFound(new { erro = "Sensor não encontrado." });

            sensor.MacAddress = model.MacAddress;
            sensor.Status = model.Status;

            _context.Sensores.Update(sensor);
            await _context.SaveChangesAsync();

            return Ok(new { mensagem = "Sensor atualizado." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var sensor = await _context.Sensores.FirstOrDefaultAsync(x => x.Id == id);
            if (sensor == null) return NotFound();

            _context.Sensores.Remove(sensor);
            await _context.SaveChangesAsync();
            return Ok(new { mensagem = "Sensor removido." });
        }
    }
}