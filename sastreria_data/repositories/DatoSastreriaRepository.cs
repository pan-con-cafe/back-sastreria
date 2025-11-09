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
    public class DatoSastreriaRepository : IDatoSastreriaRepository
    {
        private readonly _dbContext _context;

        public DatoSastreriaRepository(_dbContext context)
        {
            _context = context;
        }

        public async Task<List<DatoSastreriaDomain>> GetAllAsync()
        {
            return await _context.Datos
                .Select(d => new DatoSastreriaDomain
                {
                    IdDatos = d.IdDatos,
                    Nombre = d.Nombre,
                    Telefono = d.Telefono,
                    Direccion = d.Direccion,
                    Descripcion = d.Descripcion,
                    LogoSastreria = d.LogoSastreria
                }).ToListAsync();
        }

        public async Task<DatoSastreriaDomain?> GetByIdAsync(int id)
        {
            var dato = await _context.Datos.FindAsync(id);
            if (dato == null) return null;

            return new DatoSastreriaDomain
            {
                IdDatos = dato.IdDatos,
                Nombre = dato.Nombre,
                Telefono = dato.Telefono,
                Direccion = dato.Direccion,
                Descripcion = dato.Descripcion,
                LogoSastreria = dato.LogoSastreria
            };
        }

        public async Task<DatoSastreriaDomain> CreateAsync(DatoSastreriaDomain datoDomain)
        {
            var dato = new Dato
            {
                Nombre = datoDomain.Nombre,
                Telefono = datoDomain.Telefono,
                Direccion = datoDomain.Direccion,
                Descripcion = datoDomain.Descripcion,
                LogoSastreria = datoDomain.LogoSastreria
            };
            _context.Datos.Add(dato);
            await _context.SaveChangesAsync();

            datoDomain.IdDatos = dato.IdDatos;
            return datoDomain;
        }

        public async Task UpdateAsync(int id, DatoSastreriaDomain datoDomain)
        {
            var dato = await _context.Datos.FindAsync(id);
            if (dato == null) return;

            dato.Nombre = datoDomain.Nombre;
            dato.Telefono = datoDomain.Telefono;
            dato.Direccion = datoDomain.Direccion;
            dato.Descripcion = datoDomain.Descripcion;
            dato.LogoSastreria = datoDomain.LogoSastreria;

            _context.Datos.Update(dato);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var dato = await _context.Datos.FindAsync(id);
            if (dato == null) return;

            _context.Datos.Remove(dato);
            await _context.SaveChangesAsync();
        }
    }
}
