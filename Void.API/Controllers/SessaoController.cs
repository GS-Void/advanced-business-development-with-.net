using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using Void.API.Data;
using Void.API.DTOs;
using Void.API.Models;

namespace Void.API.Controllers
{
    [Route("api/sessao")]
    [ApiController]
    [Authorize]
    public class SessaoController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public SessaoController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Listar todas as sessões")]
        public async Task<IActionResult> Get()
        {
            var sessoes = await _context.Sessoes.ToListAsync();
            if (!sessoes.Any()) return NoContent();

            var response = sessoes.Select(s => new SessaoResponseDTO
            {
                PacienteId = s.PacienteId,
                DataSessao = s.DataSessao,
                DesgasteAcumulado = s.DesgasteAcumulado,
                AlertaFadigaCritica = s.AlertaFadigaCritica,
                IdFisio = s.IdFisio,
                IdProtocolo = s.IdProtocolo,
                StatusSessao = s.StatusSessao
            });

            return Ok(response);
        }

        // Rota recebendo dois parâmetros
        [HttpGet("{pacienteId}/{dataSessao}")]
        [SwaggerOperation(Summary = "Obter sessão específica", Description = "Informe o ID do paciente e a data da sessão (Formato: AAAA-MM-DD)")]
        public async Task<IActionResult> Get(int pacienteId, DateTime dataSessao)
        {
            var sessao = await _context.Sessoes
                .FirstOrDefaultAsync(x => x.PacienteId == pacienteId && x.DataSessao.Date == dataSessao.Date);

            if (sessao == null) return NotFound(new { erro = "Sessão não localizada." });

            return Ok(new SessaoResponseDTO
            {
                PacienteId = sessao.PacienteId,
                DataSessao = sessao.DataSessao,
                DesgasteAcumulado = sessao.DesgasteAcumulado,
                AlertaFadigaCritica = sessao.AlertaFadigaCritica,
                IdFisio = sessao.IdFisio,
                IdProtocolo = sessao.IdProtocolo,
                StatusSessao = sessao.StatusSessao
            });
        }

        [HttpPost]
        [Authorize(Roles = "Fisioterapeuta")]
        [SwaggerOperation(Summary = "Agendar nova sessão")]
        public async Task<IActionResult> Post([FromBody] SessaoRequestDTO model)
        {
            // Verifica se o paciente e fisio existem
            var pacienteExiste = await _context.Pacientes.AnyAsync(p => p.Id == model.PacienteId);
            var fisioExiste = await _context.Fisioterapeutas.AnyAsync(f => f.Id == model.IdFisio);

            if (!pacienteExiste || !fisioExiste)
                return BadRequest(new { erro = "Paciente ou Fisioterapeuta inválido." });

            var novaSessao = new SessaoReabilitacaoEntity
            {
                PacienteId = model.PacienteId,
                DataSessao = model.DataSessao,
                IdFisio = model.IdFisio,
                IdProtocolo = model.IdProtocolo,
                StatusSessao = model.StatusSessao,
                DesgasteAcumulado = 0, 
                AlertaFadigaCritica = 0
            };

            _context.Sessoes.Add(novaSessao);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { pacienteId = novaSessao.PacienteId, dataSessao = novaSessao.DataSessao.ToString("yyyy-MM-dd") }, model);
        }

        [HttpDelete("{pacienteId}/{dataSessao}")]
        [Authorize(Roles = "Fisioterapeuta")]
        [SwaggerOperation(Summary = "Cancelar ou deletar sessão")]
        public async Task<IActionResult> Delete(int pacienteId, DateTime dataSessao)
        {
            var sessao = await _context.Sessoes
                .FirstOrDefaultAsync(x => x.PacienteId == pacienteId && x.DataSessao.Date == dataSessao.Date);

            if (sessao == null) return NotFound(new { erro = "Sessão não localizada." });

            _context.Sessoes.Remove(sessao);
            await _context.SaveChangesAsync();

            return Ok(new { mensagem = "Sessão removida com sucesso." });
        }
    }
}