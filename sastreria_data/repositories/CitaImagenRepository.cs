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
    public class CitaImagenRepository : ICitaImagenRepository
    {
        private readonly _dbContext _context;

        public CitaImagenRepository(_dbContext context)
        {
            _context = context;
        }

        public async Task<CitaImagenDomain> CreateAsync(CitaImagenDomain citaImagenDomain)
        {
            var citaImagen = new CitaImagen
            {
                IdCita = citaImagenDomain.IdCita,
                Url = citaImagenDomain.Url
            };

            _context.CitaImagen.Add(citaImagen);
            await _context.SaveChangesAsync();

            citaImagenDomain.IdCitaImagen = citaImagen.IdCitaImagen;
            return citaImagenDomain;
        }

        public async Task<List<CitaImagenDomain>> GetByCitaAsync(int idCita)
        {
            return await _context.CitaImagen
                .Where(ci => ci.IdCita == idCita)
                .Select(ci => new CitaImagenDomain
                {
                    IdCitaImagen = ci.IdCitaImagen,
                    IdCita = ci.IdCita,
                    Url = ci.Url
                })
                .ToListAsync();
        }
    }
}
