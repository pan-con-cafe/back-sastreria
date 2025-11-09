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
    public class TipoDocumentoRepository : ITipoDocumentoRepository
    {
        private readonly _dbContext _context;

        public TipoDocumentoRepository(_dbContext context)
        {
            _context = context;
        }

        public async Task<List<TipoDocumentoDomain>> GetAllAsync()
        {
            return await _context.TipoDocumentos
                .Select(td => new TipoDocumentoDomain
                {
                    IdTipoDocumento = td.IdTipoDocumento,
                    Nombre = td.Nombre
                })
                .ToListAsync();
        }

        public async Task<TipoDocumentoDomain?> GetByIdAsync(int id)
        {
            var tipo = await _context.TipoDocumentos.FindAsync(id);
            if (tipo == null) return null;

            return new TipoDocumentoDomain
            {
                IdTipoDocumento = tipo.IdTipoDocumento,
                Nombre = tipo.Nombre
            };
        }

        public async Task<TipoDocumentoDomain> CreateAsync(TipoDocumentoDomain tipoDocumentoDomain)
        {
            var tipo = new TipoDocumento
            {
                Nombre = tipoDocumentoDomain.Nombre
            };
            _context.TipoDocumentos.Add(tipo);
            await _context.SaveChangesAsync();

            tipoDocumentoDomain.IdTipoDocumento = tipo.IdTipoDocumento;
            return tipoDocumentoDomain;
        }

        public async Task UpdateAsync(int id, TipoDocumentoDomain tipoDocumentoDomain)
        {
            var tipo = await _context.TipoDocumentos.FindAsync(id);
            if (tipo == null) return;

            tipo.Nombre = tipoDocumentoDomain.Nombre;
            _context.TipoDocumentos.Update(tipo);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var tipo = await _context.TipoDocumentos.FindAsync(id);
            if (tipo == null) return;

            _context.TipoDocumentos.Remove(tipo);
            await _context.SaveChangesAsync();
        }
    }
}
