using Microsoft.AspNetCore.Mvc;
using sastreria_domain.entities;
using sastreria_domain.repositories;

namespace sastreria_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CitaImagenController : ControllerBase
    {
        private readonly ICitaImagenRepository _citaImagenRepository;

        public CitaImagenController(ICitaImagenRepository citaImagenRepository)
        {
            _citaImagenRepository = citaImagenRepository;
        }

        // ✅ POST: api/CitaImagen
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CitaImagenDomain model)
        {
            if (model.IdCita <= 0 || string.IsNullOrEmpty(model.Url))
            {
                return BadRequest("Datos inválidos");
            }

            var result = await _citaImagenRepository.CreateAsync(model);
            return Ok(result);
        }

        // ✅ GET: api/CitaImagen/cita/5
        [HttpGet("cita/{idCita}")]
        public async Task<IActionResult> GetByCita(int idCita)
        {
            var imagenes = await _citaImagenRepository.GetByCitaAsync(idCita);
            return Ok(imagenes);
        }
    }
}
