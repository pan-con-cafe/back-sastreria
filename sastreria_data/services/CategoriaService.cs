
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using sastreria_data.database.tables;
using sastreria_domain.services;
using WebSastreria.models;
using sastreria_domain.Errors;
using sastreria_data.database.tables;

namespace sastreria_data.services
{
    public class CategoriaService : ICategoriaService
    {
        private readonly _dbContext _db;
        private readonly IMapper _mapper;

        public CategoriaService(_dbContext db,  IMapper mapper) {
            _db = db;
            _mapper = mapper;
        }


		#region CRUD
        public async Task<List<CategoriaResponse>> GetAllAsync()
        {
            List<Categoria> categorias = await _db.Categoria.ToListAsync();
        
            return _mapper.Map<List<CategoriaResponse>>(categorias);
        }


        public async Task<CategoriaResponse?> GetByIdAsync(int id)
        {
            /*Categoria categoria = await _db.Categoria.FirstOrDefaultAsync( x => x.IdCategoria == id && x.Estado) ?? throw new Exception("No se encontró la categoría");

            CategoriaResponse response = _mapper.Map<CategoriaResponse>(categoria); 
            return response;*/

            Categoria? categoria = _db.Categoria
                .FirstOrDefault(r => r.IdCategoria == id && r.Estado);

            if (categoria == null) return null;
            CategoriaResponse response = _mapper.Map<CategoriaResponse>(categoria); 
            return response;

        }


		public async Task<CategoriaResponse> CreateAsync(CategoriaResponse entity)
        {
            if (await _db.Categoria.AnyAsync(c => c.Nombre == entity.Nombre))
                throw new MessageExeption("El nombre de la categoría ya existe");

            Categoria categoria = _mapper.Map<Categoria>(entity);
           // categoria.Estado = true;

            _db.Categoria.Add(categoria);

            int r = await _db.SaveChangesAsync();

            entity.IdCategoria = (r == 1) ? categoria.IdCategoria : throw new MessageExeption("No se pudo insertar esta categoria");

            return  entity;
        }

        public async Task UpdateAsync(int id, CategoriaResponse entity)
        {
            var categoriaTable = await _db.Categoria.FirstOrDefaultAsync(x => x.IdCategoria == id && x.Estado) 
                ?? throw new MessageExeption("No se encontró la categoría");
        
            // Aplica cambios controlados
            categoriaTable.Nombre = entity.Nombre;
            categoriaTable.Descripcion = entity.Descripcion;
        
            if (await _db.SaveChangesAsync() != 1)
                throw new MessageExeption("No se pudo editar la categoría");
        }


        public async Task DeleteAsync(int id)
        {
            Categoria categoria = await _db.Categoria.FirstOrDefaultAsync(x => x.IdCategoria == id) ?? throw new MessageExeption("No se encontró la categoría");
            
            _db.Categoria.Remove(categoria);

            if (await _db.SaveChangesAsync() != 1)
                throw new MessageExeption("No se pudo eliminar la categoría");
        }

        

        public async Task LogicDeleteAsync(int id)
        {
            Categoria categoria = await _db.Categoria.FirstOrDefaultAsync( x => x.IdCategoria == id && x.Estado ) ?? throw new MessageExeption("No se encontró la categoría");

            categoria.Estado = false;

            if (await _db.SaveChangesAsync()!=1)
                throw new MessageExeption("No se pudo eliminar la categoría");
        }
	}
	#endregion CRUD FIN
}

