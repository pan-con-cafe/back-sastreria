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
    public class SastreController : ControllerBase
    {
        private readonly ISastreRepository _sastreRepository;

        public SastreController(ISastreRepository sastreRepository)
        {
            _sastreRepository = sastreRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var sastres = await _sastreRepository.GetAllAsync();
            return Ok(sastres);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var sastre = await _sastreRepository.GetByIdAsync(id);
            if (sastre == null) return NotFound();
            return Ok(sastre);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Create(SastreDto dto)
        {
            var sastreDomain = new SastreDomain
            {
                Nombre = dto.Nombre,
                Correo = dto.Correo,
                Contrasenia = dto.Contrasenia
            };

            var created = await _sastreRepository.CreateAsync(sastreDomain);
            return Ok(created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, SastreDto dto)
        {
            var sastreDomain = new SastreDomain
            {
                Nombre = dto.Nombre,
                Correo = dto.Correo,
                Contrasenia = dto.Contrasenia
            };

            await _sastreRepository.UpdateAsync(id, sastreDomain);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _sastreRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
