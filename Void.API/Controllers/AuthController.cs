using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using Void.API.Data;
using Void.API.DTOs;
using Void.API.Services;

namespace Void.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationContext _context;
        private readonly TokenService _tokenService;

        public AuthController(ApplicationContext context, TokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        [SwaggerOperation(
            Summary = "Autenticar no sistema VOID",
            Description = "Valida as credenciais e devolve o Token JWT. <br><br> <b>👨‍⚕️ CONTA DE TESTE (FISIOTERAPEUTA - ACESSO TOTAL):</b><br> CPF: <code>66666666666</code><br> Email: <code>roberto@void.com</code><br><br> <b>🧑 CONTA DE TESTE (PACIENTE):</b><br> CPF: <code>11111111111</code><br> Email: <code>carlos@void.com</code>"
        )]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO model)
        {
            // 1º Tenta buscar na tabela de Pacientes
            var paciente = await _context.Pacientes
                .FirstOrDefaultAsync(p => p.Cpf == model.Cpf && p.Email == model.Email);

            if (paciente != null)
            {
                var token = _tokenService.GerarToken(paciente);
                return Ok(new LoginResponseDTO { Token = token, Nome = paciente.Nome, Role = "Paciente" });
            }

            // 2º Se não achou, tenta buscar na tabela de Fisioterapeutas
            var fisio = await _context.Fisioterapeutas
                .FirstOrDefaultAsync(f => f.Cpf == model.Cpf && f.Email == model.Email);

            if (fisio != null)
            {
                var token = _tokenService.GerarToken(fisio);
                return Ok(new LoginResponseDTO { Token = token, Nome = fisio.Nome, Role = "Fisioterapeuta" });
            }

            // Se não encontrou em nenhuma das duas, bloqueia
            return Unauthorized(new { erro = "Credenciais inválidas ou usuário não encontrado no sistema." });
        }
    }
}