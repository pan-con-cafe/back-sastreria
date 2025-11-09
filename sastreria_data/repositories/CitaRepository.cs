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
    public class CitaRepository : ICitaRepository
    {
        private readonly _dbContext _context;

        public CitaRepository(_dbContext context)
        {
            _context = context;
        }

        public async Task<List<CitaDomain>> GetAllAsync()
        {
            return await _context.Cita
                .Select(c => new CitaDomain
                {
                    IdCita = c.IdCita,
                    IdCliente = c.IdCliente,
                    FechaCita = c.FechaCita,
                    Estado = c.Estado,
                    Notas = c.Notas
                })
                .ToListAsync();
        }

        public async Task<CitaDomain?> GetByIdAsync(int id)
        {
            var cita = await _context.Cita.FindAsync(id);
            if (cita == null) return null;

            return new CitaDomain
            {
                IdCita = cita.IdCita,
                IdCliente = cita.IdCliente,
                FechaCita = cita.FechaCita,
                Estado = cita.Estado,
                Notas = cita.Notas
            };
        }

        public async Task<CitaDomain> CreateAsync(CitaDomain citaDomain)
        {
            var cita = new Citum
            {
                IdCliente = citaDomain.IdCliente,
                FechaCita = citaDomain.FechaCita,
                Estado = citaDomain.Estado,
                Notas = citaDomain.Notas
            };

            _context.Cita.Add(cita);
            await _context.SaveChangesAsync();

            citaDomain.IdCita = cita.IdCita;
            return citaDomain;
        }

        public async Task UpdateAsync(int id, CitaDomain citaDomain)
        {
            var cita = await _context.Cita.FindAsync(id);
            if (cita == null) return;

            cita.IdCliente = citaDomain.IdCliente;
            cita.FechaCita = citaDomain.FechaCita;
            cita.Estado = citaDomain.Estado;
            cita.Notas = citaDomain.Notas;

            _context.Cita.Update(cita);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var cita = await _context.Cita.FindAsync(id);
            if (cita == null) return;

            _context.Cita.Remove(cita);
            await _context.SaveChangesAsync();
        }
    }
}
