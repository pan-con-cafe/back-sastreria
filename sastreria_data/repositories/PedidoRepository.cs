using sastreria_data.database.tables;
using sastreria_domain.entities;
using sastreria_domain.repositories;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sastreria_data.repositories
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly _dbContext _context;

        public PedidoRepository(_dbContext context)
        {
            _context = context;
        }

        public async Task<List<PedidoDomain>> GetAllAsync()
        {
            return await _context.Pedidos
                .Select(p => new PedidoDomain
                {
                    IdPedido = p.IdPedido,
                    FechaEntrega = p.FechaEntrega,
                    Detalle = p.Detalle,
                    IdSastre = p.IdSastre,
                    IdCliente = p.IdCliente,
                    IdEstado = p.IdEstado,
                    IdModelo = p.IdModelo
                })
                .ToListAsync();
        }

        public async Task<PedidoDomain?> GetByIdAsync(int id)
        {
            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido == null) return null;

            return new PedidoDomain
            {
                IdPedido = pedido.IdPedido,
                FechaEntrega = pedido.FechaEntrega,
                Detalle = pedido.Detalle,
                IdSastre = pedido.IdSastre,
                IdCliente = pedido.IdCliente,
                IdEstado = pedido.IdEstado,
                IdModelo = pedido.IdModelo
            };
        }

        public async Task<PedidoDomain> CreateAsync(PedidoDomain pedidoDomain)
        {
            var pedido = new Pedido
            {
                FechaEntrega = pedidoDomain.FechaEntrega,
                Detalle = pedidoDomain.Detalle,
                IdSastre = pedidoDomain.IdSastre,
                IdCliente = pedidoDomain.IdCliente,
                IdEstado = pedidoDomain.IdEstado,
                IdModelo = pedidoDomain.IdModelo
            };

            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();

            pedidoDomain.IdPedido = pedido.IdPedido;
            return pedidoDomain;
        }

        public async Task UpdateAsync(int id, PedidoDomain pedidoDomain)
        {
            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido == null) throw new Exception("Pedido no encontrado");

            pedido.FechaEntrega = pedidoDomain.FechaEntrega;
            pedido.Detalle = pedidoDomain.Detalle;
            pedido.IdSastre = pedidoDomain.IdSastre;
            pedido.IdCliente = pedidoDomain.IdCliente;
            pedido.IdEstado = pedidoDomain.IdEstado;
            pedido.IdModelo = pedidoDomain.IdModelo;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido == null) throw new Exception("Pedido no encontrado");

            _context.Pedidos.Remove(pedido);
            await _context.SaveChangesAsync();
        }
    }

}
