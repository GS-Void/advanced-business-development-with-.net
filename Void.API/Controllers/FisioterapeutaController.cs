using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using Void.API.Data;
using Void.API.DTOs;
using Void.API.Models;

namespace Void.API.Controllers
{
    [Route("api/fisioterapeuta")]
    [ApiController]
    [Authorize]
    public class FisioterapeutaController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public FisioterapeutaController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Listar todos os fisioterapeutas")]
        public async Task<IActionResult> Get()
        {
            var fisios = await _context.Fisioterapeutas.ToListAsync();
            if (!fisios.Any()) return NoContent();

            var response = fisios.Select(f => new FisioterapeutaResponseDTO
            {
                Id = f.Id,
                Nome = f.Nome,
                Cpf = f.Cpf,
                Email = f.Email,
                RegistroProfissional = f.RegistroProfissional
            });

            return Ok(response);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Obter fisioterapeuta pelo ID")]
        public async Task<IActionResult> Get(int id)
        {
            var fisio = await _context.Fisioterapeutas.FirstOrDefaultAsync(x => x.Id == id);
            if (fisio == null) return NotFound(new { erro = "Fisioterapeuta não localizado." });

            return Ok(new FisioterapeutaResponseDTO
            {
                Id = fisio.Id,
                Nome = fisio.Nome,
                Cpf = fisio.Cpf,
                Email = fisio.Email,
                RegistroProfissional = fisio.RegistroProfissional
            });
        }

        [HttpPost]
        [AllowAnonymous] // Permitimos cadastro sem token (para o primeiro acesso ao sistema)
        [SwaggerOperation(Summary = "Cadastrar novo fisioterapeuta")]
        public async Task<IActionResult> Post([FromBody] FisioterapeutaRequestDTO model)
        {
            var novoFisio = new FisioterapeutaEntity
            {
                Nome = model.Nome,
                Cpf = model.Cpf,
                Email = model.Email,
                TipoUsuario = "F", 
                RegistroProfissional = model.RegistroProfissional
            };

            _context.Fisioterapeutas.Add(novoFisio);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = novoFisio.Id }, model);
        }

        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Atualizar dados do fisioterapeuta")]
        public async Task<IActionResult> Put(int id, [FromBody] FisioterapeutaRequestDTO model)
        {
            var fisio = await _context.Fisioterapeutas.FirstOrDefaultAsync(x => x.Id == id);
            if (fisio == null) return NotFound(new { erro = "Fisioterapeuta não encontrado." });

            fisio.Nome = model.Nome;
            fisio.Cpf = model.Cpf;
            fisio.Email = model.Email;
            fisio.RegistroProfissional = model.RegistroProfissional;

            _context.Fisioterapeutas.Update(fisio);
            await _context.SaveChangesAsync();

            return Ok(new { mensagem = "Cadastro atualizado com sucesso." });
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Remover fisioterapeuta")]
        public async Task<IActionResult> Delete(int id)
        {
            var fisio = await _context.Fisioterapeutas.FirstOrDefaultAsync(x => x.Id == id);
            if (fisio == null) return NotFound(new { erro = "Fisioterapeuta não localizado." });

            _context.Fisioterapeutas.Remove(fisio);
            await _context.SaveChangesAsync();

            return Ok(new { mensagem = "Fisioterapeuta excluído com sucesso." });
        }
    }
}