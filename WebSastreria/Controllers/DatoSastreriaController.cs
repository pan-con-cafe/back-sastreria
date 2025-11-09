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
    public class DatoSastreriaController : ControllerBase
    {
        private readonly IDatoSastreriaRepository _repository;

        public DatoSastreriaController(IDatoSastreriaRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var datos = await _repository.GetAllAsync();
            return Ok(datos);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            var dato = await _repository.GetByIdAsync(id);
            if (dato == null) return NotFound();
            return Ok(dato);
        }

        [HttpPost]
        public async Task<IActionResult> Create(DatoSastreriaDto dto)
        {
            var domain = new DatoSastreriaDomain
            {
                Nombre = dto.Nombre,
                Telefono = dto.Telefono,
                Direccion = dto.Direccion,
                Descripcion = dto.Descripcion,
                LogoSastreria = dto.LogoSastreria
            };
            var created = await _repository.CreateAsync(domain);
            return Ok(created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, DatoSastreriaDto dto)
        {
            var domain = new DatoSastreriaDomain
            {
                Nombre = dto.Nombre,
                Telefono = dto.Telefono,
                Direccion = dto.Direccion,
                Descripcion = dto.Descripcion,
                LogoSastreria = dto.LogoSastreria
            };
            await _repository.UpdateAsync(id, domain);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repository.DeleteAsync(id);
            return NoContent();
        }
    }
}
