using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sastreria_data.database.tables;
using sastreria_data.repositories;
using sastreria_domain.Errors;
using sastreria_domain.repositories;
using sastreria_domain.RequestResponse;
using WebSastreria.models;

namespace WebSastreria.Controllers
{
    [ApiController]
    [Route("api/Categoria")]
    //[Authorize]
    public class CategoriaController : ControllerBase
    {
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly IModeloRepository _modeloRepository;
        private readonly _dbContext _context;
        private readonly ILogger<CategoriaController> _logger;
        private readonly IMapper _mapper;

        public CategoriaController(ICategoriaRepository categoriaRepository, IModeloRepository modeloRepository, _dbContext context, ILogger<CategoriaController> logger, IMapper mapper)
        {
            _categoriaRepository = categoriaRepository;
            _modeloRepository = modeloRepository;
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<CategoriaResponse>>> GetAll()
        {
            try
            {
                var response = await _categoriaRepository.GetAllAsync();
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"[Categoria][GetAll] {ex.Message}\n {ex.StackTrace} ");
                return StatusCode(500, new CustomResponse(error: true));
            }
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<CategoriaResponse>> GetById(int id)
        {
            try
            {
                var categoria = await _categoriaRepository.GetByIdAsync(id);
                if (categoria == null)
                    return NotFound();

                return Ok(categoria);
            }
            catch (Exception ex)
            {
                _logger.LogError($"[Categoria][GetById] {ex.Message}\n{ex.StackTrace}");
                return StatusCode(500, new CustomResponse(error: true));
            }
        }

        [HttpGet("con-modelos")]
        public async Task<IActionResult> GetCategoriasConModelos()
        {
            var categorias = await _categoriaRepository.GetAllAsync();

            var resultado = new List<object>();

            foreach (var categoria in categorias)
            {
                var modelos = await _modeloRepository.GetByCategoriaAsync(categoria.IdCategoria);
                var modelosFormateados = modelos.Select(m => new
                {
                    idModelo = m.IdModelo,
                    nombre = m.Nombre,
                    imagenes = m.Imagenes?.ToList() ?? new List<string>()
                }).ToList();

                resultado.Add(new
                {
                    categoria = categoria.Nombre,
                    modelos = modelosFormateados
                });
            }

            // Agregar "Ver todo"
            var todosLosModelos = await _modeloRepository.GetAllAsync();
            var todosLosModelosFormateados = todosLosModelos.Select(m => new
            {
                idModelo = m.IdModelo,
                nombre = m.Nombre,
                imagenes = m.Imagenes?.ToList() ?? new List<string>()
            }).ToList();

            resultado.Add(new
            {
                categoria = "Ver todo",
                modelos = todosLosModelosFormateados
            });

            return Ok(resultado);
        }


        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CategoriaRequest request)
        {
            try
            {
                // 🔹 Crear la categoría base
                var categoria = _mapper.Map<Categoria>(request);
                _context.Categoria.Add(categoria);
                await _context.SaveChangesAsync();

                // 🔹 Si se enviaron modelos asociados, los vinculamos en la tabla intermedia
                if (request.IdsModelos != null && request.IdsModelos.Any())
                {
                    var modelos = await _context.Modelos
                        .Where(m => request.IdsModelos.Contains(m.IdModelo))
                        .ToListAsync();

                    foreach (var modelo in modelos)
                    {
                        modelo.Categorias.Add(categoria);
                    }

                    await _context.SaveChangesAsync();
                }

                // 🔹 Mapeo y respuesta final
                var response = _mapper.Map<CategoriaResponse>(categoria);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"[Categoria][Create] {ex.Message}\n {ex.StackTrace}");
                return StatusCode(500, new CustomResponse(error: true));
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] CategoriaRequest request)
        {
            try
            {
                var categoria = await _context.Categoria
                    .Include(c => c.Modelos)
                    .FirstOrDefaultAsync(c => c.IdCategoria == id);

                if (categoria == null)
                    return NotFound();

                // 🔹 Actualizar propiedades básicas
                categoria.Nombre = request.Nombre;
                categoria.Descripcion = request.Descripcion;
                categoria.Estado = request.Estado;

                // 🔹 Actualizar modelos asociados si se enviaron
                if (request.IdsModelos != null)
                {
                    // Quitamos las asociaciones actuales
                    categoria.Modelos.Clear();

                    // Agregamos las nuevas
                    var nuevosModelos = await _context.Modelos
                        .Where(m => request.IdsModelos.Contains(m.IdModelo))
                        .ToListAsync();

                    foreach (var modelo in nuevosModelos)
                    {
                        categoria.Modelos.Add(modelo);
                    }
                }

                await _context.SaveChangesAsync();
                return Ok(new CustomResponse());
            }
            catch (Exception ex)
            {
                _logger.LogError($"[Categoria][Update] {ex.Message}\n {ex.StackTrace}");
                return StatusCode(500, new CustomResponse(error: true));
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var categoria = await _context.Categoria
                    .Include(c => c.Modelos)
                    .FirstOrDefaultAsync(c => c.IdCategoria == id);

                if (categoria == null)
                    return NotFound();

                // 🔹 Quitar relaciones antes de eliminar
                categoria.Modelos.Clear();
                _context.Categoria.Remove(categoria);
                await _context.SaveChangesAsync();

                return Ok(new CustomResponse());
            }
            catch (Exception ex)
            {
                _logger.LogError($"[Categoria][Delete] {ex.Message}\n {ex.StackTrace}");
                return StatusCode(500, new CustomResponse(error: true));
            }
        }

    }
}
