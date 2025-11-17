using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sastreria_domain.entities;
using sastreria_domain.repositories;
using WebSastreria.Dtos;


namespace WebSastreria.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class HorarioController : ControllerBase
    {
        private readonly IHorarioRepository _horarioRepository;

        public HorarioController(IHorarioRepository horarioRepository)
        {
            _horarioRepository = horarioRepository;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var horarios = await _horarioRepository.GetAllAsync();
            return Ok(horarios);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            var horario = await _horarioRepository.GetByIdAsync(id);
            if (horario == null) return NotFound();
            return Ok(horario);
        }

        [HttpPost]
        public async Task<IActionResult> Create(HorarioDto horarioDto)
        {
            var horarioDomain = new HorarioDomain
            {
                Dia = horarioDto.Dia,
                HoraInicio = horarioDto.HoraInicio,
                HoraFin = horarioDto.HoraFin,
                Estado = horarioDto.Estado
            };

            var created = await _horarioRepository.CreateAsync(horarioDomain);
            return Ok(created);
        }

        [HttpPut("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Update(int id, [FromBody] HorarioDto horarioDto)
        {
            var horarioDomain = new HorarioDomain
            {
                Dia = horarioDto.Dia,
                HoraInicio = horarioDto.HoraInicio,
                HoraFin = horarioDto.HoraFin,
                Estado = horarioDto.Estado
            };

            await _horarioRepository.UpdateAsync(id, horarioDomain);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _horarioRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
