using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using sastreria_data.database;
using sastreria_data.database.tables;
using sastreria_domain.entities;
using sastreria_domain.repositories;


namespace sastreria_data.repositories
{
    public class EstadoRepository : IEstadoRepository
    {
        private readonly _dbContext _context;

        public EstadoRepository(_dbContext context)
        {
            _context = context;
        }

        public async Task<List<EstadoDomain>> GetAllAsync()
        {
            return await _context.Estados
                .Select(e => new EstadoDomain
                {
                    IdEstado = e.IdEstado,
                    Nombre = e.Nombre
                })
                .ToListAsync();
        }

        public async Task<EstadoDomain?> GetByIdAsync(int id)
        {
            var estado = await _context.Estados.FindAsync(id);
            if (estado == null) return null;
            return new EstadoDomain { IdEstado = estado.IdEstado, Nombre = estado.Nombre };
        }

        public async Task<EstadoDomain> CreateAsync(EstadoDomain estadoDomain)
        {
            var estado = new Estado { Nombre = estadoDomain.Nombre };
            _context.Estados.Add(estado);
            await _context.SaveChangesAsync();
            estadoDomain.IdEstado = estado.IdEstado;
            return estadoDomain;
        }

        public async Task UpdateAsync(int id, EstadoDomain estadoDomain)
        {
            var estado = await _context.Estados.FindAsync(id);
            if (estado == null) return;
            estado.Nombre = estadoDomain.Nombre;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var estado = await _context.Estados.FindAsync(id);
            if (estado == null) return;
            _context.Estados.Remove(estado);
            await _context.SaveChangesAsync();
        }
    }
}
