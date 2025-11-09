using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sastreria_data.database.tables;
using sastreria_domain.entities;
using sastreria_domain.repositories;
using sastreria_domain.RequestResponse;

namespace WebSastreria.Controllers
{
    [ApiController]
    [Route("api/Modelo")]
    //[Authorize]
    public class ModeloController : ControllerBase
    {
        private readonly IModeloRepository _modeloRepository;
        private readonly ILogger<ModeloController> _logger;
        private readonly IMapper _mapper;

        public ModeloController(IModeloRepository modeloRepository, ILogger<ModeloController> logger, IMapper mapper)
        {
            _modeloRepository = modeloRepository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<ModeloResponse>>> GetAll()
        {
            return Ok(await _modeloRepository.GetAllAsync());
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<ModeloResponse>> GetById(int id)
        {
            var modelo = await _modeloRepository.GetByIdAsync(id);
            if (modelo == null)
                return NotFound();

            var response = _mapper.Map<ModeloResponse>(modelo);
            return Ok(response);
        }



        [HttpGet("categorias")]
        [AllowAnonymous]
        public async Task<ActionResult> GetByCategoria()
        {
            var modelos = await _modeloRepository.GetAllAsync();

            // 🔹 Validamos que haya categorías cargadas
            var modelosPorCategoria = modelos
                .Where(m => m.Categorias != null && m.Categorias.Any())
                .SelectMany(m => m.Categorias!.Select(categoria => new
                {
                    Categoria = categoria,
                    Modelo = m
                }));

            // 🔹 Agrupamos por nombre de categoría
            var agrupados = modelosPorCategoria
                .GroupBy(x => x.Categoria)
                .Select(g => new
                {
                    categoria = g.Key,
                    modelos = g.Select(m => new
                    {
                        id = m.Modelo.IdModelo,
                        nombre = m.Modelo.Nombre,
                        imagen = m.Modelo.Imagenes?.FirstOrDefault()
                    }).ToList()
                });

            return Ok(agrupados);
        }




        [HttpPost]
        public async Task<ActionResult<ModeloResponse>> Create([FromBody] ModeloRequest request)
        {
            try
            {
                var modeloDomain = _mapper.Map<ModeloDomain>(request);
                var created = await _modeloRepository.CreateAsync(modeloDomain);

                var response = _mapper.Map<ModeloResponse>(created);
                return Ok(response);
            }
            catch (Exception ex)
            {
                var errorMessage = ex.InnerException?.Message ?? ex.Message;
                _logger.LogError($"[Modelo][Create] {errorMessage}\n{ex.StackTrace}");
                return StatusCode(500, $"Error al crear el modelo: {errorMessage}");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] ModeloRequest request)
        {
            try
            {
                var modeloDomain = _mapper.Map<ModeloDomain>(request);
                await _modeloRepository.UpdateAsync(id, modeloDomain);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"[Modelo][Update] {ex.Message}\n{ex.StackTrace}");
                return StatusCode(500, "Error al actualizar el modelo");
            }
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _modeloRepository.DeleteAsync(id);
            return Ok();
        }
    }

}
