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
    public class ClienteRepository : IClienteRepository
    {
        private readonly _dbContext _context;

        public ClienteRepository(_dbContext context)
        {
            _context = context;
        }

        public async Task<List<ClienteDomain>> GetAllAsync()
        {
            return await _context.Clientes
                .Select(c => new ClienteDomain
                {
                    IdCliente = c.IdCliente,
                    IdTipoDocumento = c.IdTipoDocumento,
                    Nombre = c.Nombre,
                    Apellido = c.Apellido,
                    Correo = c.Correo,
                    Telefono = c.Telefono,
                    NumeroDocumento = c.NumeroDocumento
                })
                .ToListAsync();
        }

        public async Task<ClienteDomain?> GetByIdAsync(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null) return null;

            return new ClienteDomain
            {
                IdCliente = cliente.IdCliente,
                IdTipoDocumento = cliente.IdTipoDocumento,
                Nombre = cliente.Nombre,
                Apellido = cliente.Apellido,
                Correo = cliente.Correo,
                Telefono = cliente.Telefono,
                NumeroDocumento = cliente.NumeroDocumento
            };
        }

        public async Task<ClienteDomain> CreateAsync(ClienteDomain clienteDomain)
        {
            var cliente = new Cliente
            {
                IdTipoDocumento = clienteDomain.IdTipoDocumento,
                Correo = clienteDomain.Correo,
                Telefono = clienteDomain.Telefono,
                Nombre = clienteDomain.Nombre!,
                Apellido = clienteDomain.Apellido!,
                NumeroDocumento = clienteDomain.NumeroDocumento
            };

            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();

            clienteDomain.IdCliente = cliente.IdCliente;
            return clienteDomain;
        }

        public async Task<ClienteDomain?> GetByNumeroDocumentoAsync(string numeroDocumento)
        {
            var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.NumeroDocumento == numeroDocumento);
            if (cliente == null) return null;

            return new ClienteDomain
            {
                IdCliente = cliente.IdCliente,
                Nombre = cliente.Nombre,
                Apellido = cliente.Apellido,
                Correo = cliente.Correo,
                Telefono = cliente.Telefono,
                NumeroDocumento = cliente.NumeroDocumento,
                IdTipoDocumento = cliente.IdTipoDocumento
            };
        }


        public async Task UpdateAsync(int id, ClienteDomain clienteDomain)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null) return;

            cliente.IdTipoDocumento = clienteDomain.IdTipoDocumento;
            cliente.Nombre = clienteDomain.Nombre;
            cliente.Apellido = clienteDomain.Apellido;
            cliente.Correo = clienteDomain.Correo;
            cliente.Telefono = clienteDomain.Telefono;
            cliente.NumeroDocumento = clienteDomain.NumeroDocumento;

            _context.Clientes.Update(cliente);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null) return;

            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();
        }
    }
}
