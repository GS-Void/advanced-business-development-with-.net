using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Void.API.Data;
using Void.API.DTOs;
using Void.API.Models;

namespace Void.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SessaoController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public SessaoController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SessaoRequestDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var connection = _context.Database.GetDbConnection();
            int pacienteIdEncontrado = 0;

            if (connection.State != ConnectionState.Open)
                await connection.OpenAsync();

            using (var command = connection.CreateCommand())
            {
                command.CommandText =
                    "SELECT ID_USUARIO FROM TB_VOID_PACIENTE WHERE ID_USUARIO = :id";

                var param = command.CreateParameter();
                param.ParameterName = "id";
                param.Value = model.PacienteId;
                command.Parameters.Add(param);

                var result = await command.ExecuteScalarAsync();

                if (result != null)
                {
                    pacienteIdEncontrado = Convert.ToInt32(result);
                }
            }

            if (pacienteIdEncontrado == 0)
            {
                return NotFound(new
                {
                    mensagem = "Paciente não encontrado."
                });
            }

            var sessao = new SessaoReabilitacaoEntity
            {
                PacienteId = model.PacienteId,
                DataSessao = model.DataSessao,
                IdFisio = model.IdFisio,
                IdProtocolo = model.IdProtocolo,
                StatusSessao = model.StatusSessao
            };

            _context.Sessoes.Add(sessao);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                mensagem = "Sessão criada com sucesso.",
                sessao.PacienteId,
                sessao.DataSessao,
                sessao.IdFisio,
                sessao.IdProtocolo,
                sessao.StatusSessao
            });
        }

        [HttpGet("{pacienteId}/{dataSessao}")]
        public async Task<IActionResult> GetById(
            int pacienteId,
            DateTime dataSessao)
        {
            var sessao = await _context.Sessoes.FindAsync(
                pacienteId,
                dataSessao
            );

            if (sessao == null)
                return NotFound();

            return Ok(sessao);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var sessoes = await _context.Sessoes.ToListAsync();
            return Ok(sessoes);
        }
    }
}