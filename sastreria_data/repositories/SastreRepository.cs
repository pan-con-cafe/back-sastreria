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
    public class SastreRepository : ISastreRepository
    {
        private readonly _dbContext _context;

        public SastreRepository(_dbContext context)
        {
            _context = context;
        }

        public async Task<List<SastreDomain>> GetAllAsync()
        {
            return await _context.Sastres
                .Select(s => new SastreDomain
                {
                    IdSastre = s.IdSastre,
                    Nombre = s.Nombre,
                    Correo = s.Correo,
                    Contrasenia = s.Contrasenia
                })
                .ToListAsync();
        }

        public async Task<SastreDomain?> GetByIdAsync(int id)
        {
            var sastre = await _context.Sastres.FindAsync(id);
            if (sastre == null) return null;

            return new SastreDomain
            {
                IdSastre = sastre.IdSastre,
                Nombre = sastre.Nombre,
                Correo = sastre.Correo,
                Contrasenia = sastre.Contrasenia
            };
        }

        public async Task<SastreDomain> CreateAsync(SastreDomain sastreDomain)
        {
            var sastre = new Sastre
            {
                Nombre = sastreDomain.Nombre,
                Correo = sastreDomain.Correo,
                Contrasenia = sastreDomain.Contrasenia
            };

            _context.Sastres.Add(sastre);
            await _context.SaveChangesAsync();

            sastreDomain.IdSastre = sastre.IdSastre;
            return sastreDomain;
        }

        public async Task UpdateAsync(int id, SastreDomain sastreDomain)
        {
            var sastre = await _context.Sastres.FindAsync(id);
            if (sastre == null) return;

            sastre.Nombre = sastreDomain.Nombre;
            sastre.Correo = sastreDomain.Correo;
            sastre.Contrasenia = sastreDomain.Contrasenia;

            _context.Sastres.Update(sastre);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var sastre = await _context.Sastres.FindAsync(id);
            if (sastre == null) return;

            _context.Sastres.Remove(sastre);
            await _context.SaveChangesAsync();
        }
    }
}
