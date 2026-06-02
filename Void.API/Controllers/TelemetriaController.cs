using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Void.API.Data;
using Void.API.DTOs;
using Void.API.Models;

namespace Void.API.Controllers
{
    [Route("api/telemetria")]
    [ApiController]
    public class TelemetriaController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public TelemetriaController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpPost("ingestao")]
        [AllowAnonymous] 
        [SwaggerOperation(Summary = "Ingestão de dados do ESP32", Description = "Recebe o JSON puro do microcontrolador e salva no banco Oracle.")]
        public async Task<IActionResult> Post([FromBody] TelemetriaIoTRequestDTO model)
        {
            // Valida se a sessão existe e está em andamento antes de aceitar os dados
            var sessaoValida = _context.Sessoes.Any(s =>
                s.PacienteId == model.PacienteId &&
                s.DataSessao.Date == model.DataSessao.Date &&
                s.StatusSessao == "ANDAMENTO");

            if (!sessaoValida)
                return BadRequest(new { erro = "Sessão inválida, não encontrada ou não está em andamento." });

            var telemetria = new TelemetriaRawJsonEntity
            {
                PacienteId = model.PacienteId,
                DataSessao = model.DataSessao,
                DadosJson = model.DadosJson 
            };

            _context.TelemetriaLogs.Add(telemetria);
            await _context.SaveChangesAsync();

            return Ok(new { mensagem = "Payload IoT recebido e armazenado com sucesso." });
        }
    }
}