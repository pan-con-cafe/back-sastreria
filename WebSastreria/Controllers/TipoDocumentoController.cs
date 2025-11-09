using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sastreria_domain.entities;
using sastreria_domain.repositories;

namespace WebSastreria.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TipoDocumentoController : ControllerBase
    {
        private readonly ITipoDocumentoRepository _tipoDocumentoRepository;

        public TipoDocumentoController(ITipoDocumentoRepository tipoDocumentoRepository)
        {
            _tipoDocumentoRepository = tipoDocumentoRepository;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var tipos = await _tipoDocumentoRepository.GetAllAsync();
            return Ok(tipos);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            var tipo = await _tipoDocumentoRepository.GetByIdAsync(id);
            if (tipo == null) return NotFound();
            return Ok(tipo);
        }

        [HttpPost]
        public async Task<IActionResult> Create(TipoDocumentoDomain tipoDocumentoDomain)
        {
            try
            {
                var created = await _tipoDocumentoRepository.CreateAsync(tipoDocumentoDomain);
                return Ok(created);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al crear el tipo de documento: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, TipoDocumentoDomain tipoDocumentoDomain)
        {
            try
            {
                await _tipoDocumentoRepository.UpdateAsync(id, tipoDocumentoDomain);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al actualizar el tipo de documento: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _tipoDocumentoRepository.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al eliminar el tipo de documento: {ex.Message}");
            }
        }
    }
}
