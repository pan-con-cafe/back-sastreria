using Microsoft.EntityFrameworkCore;
using sastreria_data.database;
using sastreria_data.database.tables;
using sastreria_domain.entities;
using sastreria_domain.repositories;
using sastreria_domain.services;

namespace sastreria_data.repositories
{
    public class ModeloRepository : IModeloRepository
    {
        private readonly _dbContext _context;
        private readonly ICategoriaService _categoriaService;

        public ModeloRepository(_dbContext context, ICategoriaService categoriaService)
        {
            _context = context;
            _categoriaService = categoriaService;
        }

        public ICategoriaService Categoria()
        {
            return _categoriaService;
        }

        // ✅ Obtener todos los modelos
        public async Task<List<ModeloDomain>> GetAllAsync()
        {
            return await _context.Modelos
                .Include(m => m.Categorias)
                .Include(m => m.ModeloImagenes)
                .Select(m => new ModeloDomain
                {
                    IdModelo = m.IdModelo,
                    Nombre = m.Nombre,
                    Descripcion = m.Descripcion,
                    FechaCreacion = m.FechaCreacion,
                    Imagenes = m.ModeloImagenes.Select(img => img.Url).ToList(),
                    // ✅ Ahora un modelo puede tener varias categorías
                    Categorias = m.Categorias.Select(c => c.Nombre).ToList()
                })
                .ToListAsync();
        }

        // ✅ Obtener modelos por categoría (ahora mediante la relación N:N)
        public async Task<IEnumerable<ModeloDomain>> GetByCategoriaAsync(int idCategoria)
        {
            return await _context.Modelos
                .Where(m => m.Categorias.Any(c => c.IdCategoria == idCategoria))
                .Include(m => m.ModeloImagenes)
                .Select(m => new ModeloDomain
                {
                    IdModelo = m.IdModelo,
                    Nombre = m.Nombre,
                    Descripcion = m.Descripcion,
                    FechaCreacion = m.FechaCreacion,
                    Imagenes = m.ModeloImagenes.Select(img => img.Url).ToList(),
                    Categorias = m.Categorias.Select(c => c.Nombre).ToList()
                })
                .ToListAsync();
        }

        // ✅ Obtener modelo por ID
        public async Task<ModeloDomain?> GetByIdAsync(int id)
        {
            var modelo = await _context.Modelos
                .Include(m => m.Categorias)
                .Include(m => m.ModeloImagenes)
                .FirstOrDefaultAsync(m => m.IdModelo == id);

            if (modelo == null) return null;

            return new ModeloDomain
            {
                IdModelo = modelo.IdModelo,
                Nombre = modelo.Nombre,
                Descripcion = modelo.Descripcion,
                FechaCreacion = modelo.FechaCreacion,
                Imagenes = modelo.ModeloImagenes.Select(img => img.Url).ToList(),
                Categorias = modelo.Categorias.Select(c => c.Nombre).ToList()
            };
        }

        // ✅ Crear modelo con varias categorías e imágenes
        public async Task<ModeloDomain> CreateAsync(ModeloDomain modeloDomain)
        {
            if (modeloDomain.Imagenes == null || !modeloDomain.Imagenes.Any())
                throw new Exception("Debe incluir al menos una imagen.");

            var modelo = new Modelo
            {
                Nombre = modeloDomain.Nombre,
                Descripcion = modeloDomain.Descripcion,
                FechaCreacion = modeloDomain.FechaCreacion ?? DateTime.UtcNow
            };

            // Asignar categorías (N:N)
            if (modeloDomain.CategoriasIds != null && modeloDomain.CategoriasIds.Any())
            {
                var categorias = await _context.Categoria
                    .Where(c => modeloDomain.CategoriasIds.Contains(c.IdCategoria))
                    .ToListAsync();

                modelo.Categorias = categorias;
            }

            await _context.Modelos.AddAsync(modelo);
            await _context.SaveChangesAsync();

            // Asignar imágenes
            var imagenes = modeloDomain.Imagenes.Select(url => new ModeloImagen
            {
                Url = url,
                IdModelo = modelo.IdModelo
            }).ToList();

            await _context.ModeloImagen.AddRangeAsync(imagenes);
            await _context.SaveChangesAsync();

            modeloDomain.IdModelo = modelo.IdModelo;
            return modeloDomain;
        }

        // ✅ Actualizar modelo (categorías e imágenes)
        public async Task UpdateAsync(int id, ModeloDomain modeloDomain)
        {
            var modelo = await _context.Modelos
                .Include(m => m.Categorias)
                .Include(m => m.ModeloImagenes)
                .FirstOrDefaultAsync(m => m.IdModelo == id);

            if (modelo == null) return;

            modelo.Nombre = modeloDomain.Nombre;
            modelo.Descripcion = modeloDomain.Descripcion;
            modelo.FechaCreacion = modeloDomain.FechaCreacion ?? modelo.FechaCreacion;

            // Actualizar categorías
            if (modeloDomain.CategoriasIds != null)
            {
                var categorias = await _context.Categoria
                    .Where(c => modeloDomain.CategoriasIds.Contains(c.IdCategoria))
                    .ToListAsync();

                modelo.Categorias = categorias;
            }

            // Actualizar imágenes
            _context.ModeloImagen.RemoveRange(modelo.ModeloImagenes);

            if (modeloDomain.Imagenes != null && modeloDomain.Imagenes.Any())
            {
                var imagenes = modeloDomain.Imagenes.Select(url => new ModeloImagen
                {
                    Url = url,
                    IdModelo = modelo.IdModelo
                }).ToList();

                _context.ModeloImagen.AddRange(imagenes);
            }

            _context.Modelos.Update(modelo);
            await _context.SaveChangesAsync();
        }

        // ✅ Eliminar modelo e imágenes asociadas
        public async Task DeleteAsync(int id)
        {
            var modelo = await _context.Modelos
                .Include(m => m.ModeloImagenes)
                .FirstOrDefaultAsync(m => m.IdModelo == id);

            if (modelo == null) return;

            _context.ModeloImagen.RemoveRange(modelo.ModeloImagenes);
            _context.Modelos.Remove(modelo);
            await _context.SaveChangesAsync();
        }
    }
}
