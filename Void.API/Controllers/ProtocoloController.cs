using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using Void.API.Data;
using Void.API.DTOs;
using Void.API.Models;

namespace Void.API.Controllers
{
    [Route("api/protocolo")]
    [ApiController]
    [Authorize]
    public class ProtocoloController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public ProtocoloController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: Listar todos
        [HttpGet]
        [SwaggerOperation(Summary = "Listar protocolos de reabilitação")]
        public async Task<IActionResult> Get()
        {
            var protocolos = await _context.Protocolos.ToListAsync();
            if (!protocolos.Any()) return NoContent();

            return Ok(protocolos.Select(p => new ProtocoloResponseDTO
            {
                Id = p.Id,
                NomeProtocolo = p.NomeProtocolo,
                LimiteFadigaMaxima = p.LimiteFadigaMaxima
            }));
        }

        // GET: Buscar por ID
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Obter protocolo pelo ID")]
        public async Task<IActionResult> Get(int id)
        {
            var protocolo = await _context.Protocolos.FirstOrDefaultAsync(x => x.Id == id);

            if (protocolo == null)
                return NotFound(new { erro = "Protocolo não localizado." });

            return Ok(new ProtocoloResponseDTO
            {
                Id = protocolo.Id,
                NomeProtocolo = protocolo.NomeProtocolo,
                LimiteFadigaMaxima = protocolo.LimiteFadigaMaxima
            });
        }

        // POST: Criar novo
        [HttpPost]
        [Authorize(Roles = "Fisioterapeuta")]
        [SwaggerOperation(Summary = "Criar novo protocolo de carga")]
        public async Task<IActionResult> Post([FromBody] ProtocoloRequestDTO model)
        {
            var protocolo = new ProtocoloEspacialEntity
            {
                NomeProtocolo = model.NomeProtocolo,
                LimiteFadigaMaxima = model.LimiteFadigaMaxima
            };

            _context.Protocolos.Add(protocolo);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = protocolo.Id }, model);
        }

        // PUT: Atualizar existente
        [HttpPut("{id}")]
        [Authorize(Roles = "Fisioterapeuta")]
        [SwaggerOperation(Summary = "Atualizar protocolo de carga existente")]
        public async Task<IActionResult> Put(int id, [FromBody] ProtocoloRequestDTO model)
        {
            var protocolo = await _context.Protocolos.FirstOrDefaultAsync(x => x.Id == id);

            if (protocolo == null)
                return NotFound(new { erro = "Protocolo não encontrado." });

            protocolo.NomeProtocolo = model.NomeProtocolo;
            protocolo.LimiteFadigaMaxima = model.LimiteFadigaMaxima;

            _context.Protocolos.Update(protocolo);
            await _context.SaveChangesAsync();

            return Ok(new { mensagem = "Protocolo atualizado com sucesso." });
        }

        // DELETE: Excluir
        [HttpDelete("{id}")]
        [Authorize(Roles = "Fisioterapeuta")]
        [SwaggerOperation(Summary = "Remover protocolo")]
        public async Task<IActionResult> Delete(int id)
        {
            var protocolo = await _context.Protocolos.FirstOrDefaultAsync(x => x.Id == id);

            if (protocolo == null)
                return NotFound(new { erro = "Protocolo não localizado." });

            _context.Protocolos.Remove(protocolo);
            await _context.SaveChangesAsync();

            return Ok(new { mensagem = "Protocolo excluído com sucesso." });
        }
    }
}