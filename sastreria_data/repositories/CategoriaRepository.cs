using AutoMapper;
using Microsoft.EntityFrameworkCore;
using sastreria_data.database;
using sastreria_data.database.tables;
using sastreria_domain.repositories;
using sastreria_domain.RequestResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSastreria.models;

namespace sastreria_data.repositories
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly _dbContext _context;
        private readonly IMapper _mapper;

        public CategoriaRepository(_dbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<CategoriaResponse>> GetAllAsync()
        {
            var categorias = await _context.Categoria.ToListAsync();
            return _mapper.Map<List<CategoriaResponse>>(categorias);
        }

        public async Task<CategoriaResponse?> GetByIdAsync(int id)
        {
            var categoria = await _context.Categoria.FindAsync(id);
            return categoria == null ? null : _mapper.Map<CategoriaResponse>(categoria);
        }

        public async Task<CategoriaResponse> CreateAsync(CategoriaResponse categoria)
        {
            var entity = _mapper.Map<Categoria>(categoria);
            _context.Categoria.Add(entity);
            await _context.SaveChangesAsync();
            return _mapper.Map<CategoriaResponse>(entity);
        }

        public async Task UpdateAsync(int id, CategoriaResponse categoria)
        {
            var entity = await _context.Categoria.FindAsync(id);
            if (entity == null)
                throw new Exception($"Categoría {id} no encontrada");

            entity.Nombre = categoria.Nombre;
            _context.Categoria.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Categoria.FindAsync(id);
            if (entity == null)
                throw new Exception($"Categoría {id} no encontrada");

            _context.Categoria.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}

