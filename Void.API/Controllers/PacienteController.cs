using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using Void.API.Data;
using Void.API.DTOs;
using Void.API.Models;

namespace Void.API.Controllers
{
    [Route("api/paciente")]
    [ApiController]
    [Authorize] 
    public class PacienteController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public PacienteController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: Retorna Todos
        [HttpGet]
        [SwaggerOperation(Summary = "Listar todos os pacientes")]
        public async Task<IActionResult> Get()
        {
            // Busca no banco 
            var pacientes = await _context.Pacientes.ToListAsync();
            if (!pacientes.Any()) return NoContent();

            var response = pacientes.Select(p => new PacienteResponseDTO
            {
                Id = p.Id,
                Nome = p.Nome,
                Cpf = p.Cpf,
                Email = p.Email,
                LimiteEsforcoCritico = p.LimiteEsforcoCritico
            });

            return Ok(response);
        }

        // GET: Retorna por ID
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Obter paciente pelo ID")]
        public async Task<IActionResult> Get(int id)
        {
            var paciente = await _context.Pacientes.FirstOrDefaultAsync(x => x.Id == id);

            if (paciente == null)
                return NotFound(new { erro = "Paciente não localizado na base de dados." });

            var response = new PacienteResponseDTO
            {
                Id = paciente.Id,
                Nome = paciente.Nome,
                Cpf = paciente.Cpf,
                Email = paciente.Email,
                LimiteEsforcoCritico = paciente.LimiteEsforcoCritico
            };

            return Ok(response);
        }

        // POST: Adicionar Paciente
        [HttpPost]
        [Authorize(Roles = "Fisioterapeuta")] 
        [SwaggerOperation(Summary = "Cadastrar novo paciente", Description = "Requer token com perfil de Fisioterapeuta.")]
        public async Task<IActionResult> Post([FromBody] PacienteRequestDTO model)
        {
            // Mapeia do DTO recebido para a Entity do Oracle
            var novoPaciente = new PacienteEntity
            {
                Nome = model.Nome,
                Cpf = model.Cpf,
                Email = model.Email,
                TipoUsuario = "P", 
                LimiteEsforcoCritico = model.LimiteEsforcoCritico
            };

            _context.Pacientes.Add(novoPaciente);

          
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = novoPaciente.Id }, model);
        }

        // PUT: Editar Paciente
        [HttpPut("{id}")]
        [Authorize(Roles = "Fisioterapeuta")]
        [SwaggerOperation(Summary = "Atualizar dados clínicos ou cadastrais do paciente")]
        public async Task<IActionResult> Put(int id, [FromBody] PacienteRequestDTO model)
        {
            var paciente = await _context.Pacientes.FirstOrDefaultAsync(x => x.Id == id);

            if (paciente == null)
                return NotFound(new { erro = "Paciente não encontrado para atualização." });

            // Atualiza os dados
            paciente.Nome = model.Nome;
            paciente.Cpf = model.Cpf;
            paciente.Email = model.Email;
            paciente.LimiteEsforcoCritico = model.LimiteEsforcoCritico;

            _context.Pacientes.Update(paciente);
            await _context.SaveChangesAsync();

            return Ok(new { mensagem = "Cadastro atualizado com sucesso." });
        }

        // DELETE: Remover Paciente
        [HttpDelete("{id}")]
        [Authorize(Roles = "Fisioterapeuta")]
        [SwaggerOperation(Summary = "Remover paciente do programa")]
        public async Task<IActionResult> Delete(int id)
        {
            var paciente = await _context.Pacientes.FirstOrDefaultAsync(x => x.Id == id);

            if (paciente == null)
                return NotFound(new { erro = "Paciente não localizado." });

            _context.Pacientes.Remove(paciente);
            await _context.SaveChangesAsync();

            return Ok(new { mensagem = "Paciente excluído com sucesso do sistema." });
        }
    }
}