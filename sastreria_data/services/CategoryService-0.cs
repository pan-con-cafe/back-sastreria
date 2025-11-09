/*using hotel_data.sources.BaseDeDatos;
using hotel_domain.models;
using hotel_domain.services;
using hotel_data.extentions;
using hotel_data.sources.BaseDeDatos.Tables;
using hotel_domain.Errors;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace sastreria_data.services{

     public class CategoryService : ICategoryService
    {
        private readonly HotelDbContext _db;
        private readonly IMapper _mapper;

        public CategoryService(
            HotelDbContext hotelDbContext,
            IMapper mapper) {
            _db = hotelDbContext;
            _mapper = mapper;
        }

        public async Task<Categoria> CreateAsync(Categoria entity)
        {
            if (await _db.categorias.AnyAsync(c => c.Name == entity.Name))
                throw new MessageExeption("El nombre de la categor�a ya existe");

            CategoriaTable categoriaTable = entity.ToTable(_mapper) ;




            _db.categorias.Add(categoriaTable);
            int r =await _db.SaveChangesAsync();

            entity.Id = (r == 1) ? categoriaTable.Id : throw new MessageExeption("No se pudo insertar esta categoria");

            return  entity;
        }


        public async Task LogicDeleteAsync(int id)
        {
            CategoriaTable categoriaTable = await _db.categorias.FirstOrDefaultAsync( x => x.Id == id && x.State ) ?? throw new MessageExeption("No se encontr� la categor�a");

            categoriaTable.State = false;

            if (await _db.SaveChangesAsync()!=1)
                throw new MessageExeption("No se pudo eliminar la categor�a");
        }


        public async Task DeleteAsync(int id)
        {
            CategoriaTable categoriaTable = await _db.categorias.FirstOrDefaultAsync(x => x.Id == id && x.State) ?? throw new MessageExeption("No se encontr� la categor�a");
            _db.categorias.Remove(categoriaTable);

            if (await _db.SaveChangesAsync() != 1)
                throw new MessageExeption("No se pudo eliminar la categor�a");
        }

        public async Task<List<Categoria>> GetAllAsync()
        {
            List<Categoria> categorias = await _db.categorias
                .Where(x => x.State)
                .Select<CategoriaTable, Categoria>( x => x.ToModel(_mapper))
                .ToListAsync();

            return categorias;
        }

        public async Task<Categoria?> GetByIdAsync(int id)
        {
            CategoriaTable categoriaTable = await _db.categorias.FirstOrDefaultAsync( x => x.Id == id && x.State) ?? throw new ("No se encontr� la categor�a");
            return categoriaTable.ToModel(_mapper);
        }

        public async Task UpdateAsync(int id, Categoria entity)
        {
            var categoriaTable = await _db.categorias.FirstOrDefaultAsync(x => x.Id == id && x.State) 
                ?? throw new MessageExeption("No se encontr� la categor�a");
        
            // Aplica cambios controlados
            categoriaTable.Name = entity.Name;
            categoriaTable.Description = entity.Description;
        
            if (await _db.SaveChangesAsync() != 1)
                throw new MessageExeption("No se pudo editar la categor�a");
        }

        
    }

}*/