using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sastreria_domain.entities;
using sastreria_domain.repositories;
using WebSastreria.Dtos;
using System.Threading.Tasks;

namespace WebSastreria.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EstadoController : ControllerBase
    {
        private readonly IEstadoRepository _estadoRepository;

        public EstadoController(IEstadoRepository estadoRepository)
        {
            _estadoRepository = estadoRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _estadoRepository.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var estado = await _estadoRepository.GetByIdAsync(id);
            if (estado == null) return NotFound();
            return Ok(estado);
        }

        [HttpPost]
        public async Task<IActionResult> Create(EstadoDto dto)
        {
            var domain = new EstadoDomain { Nombre = dto.Nombre };
            var created = await _estadoRepository.CreateAsync(domain);
            return Ok(created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, EstadoDto dto)
        {
            await _estadoRepository.UpdateAsync(id, new EstadoDomain { Nombre = dto.Nombre });
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _estadoRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
