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
                Notas = cita.Notas,
                IdHorario = cita.IdHorario
            };
        }

        /*public async Task<CitaDomain?> GetByPedidoIdAsync(int idPedido)
        {
            var cita = await _context.Cita
                .FirstOrDefaultAsync(c => c.PedidoId == idPedido);

            if (cita == null) return null;

            return new CitaDomain
            {
                IdCita = cita.IdCita,
                IdCliente = cita.IdCliente,
                FechaCita = cita.FechaCita,
                Estado = cita.Estado,
                Notas = cita.Notas,
                PedidoId = cita.PedidoId,
            };
        }*/

        public async Task<List<CitaDomain>> GetByPedidoIdAsync(int pedidoId)
        {
            return await _context.Cita
                .Where(c => c.PedidoId == pedidoId)
                .OrderBy(c => c.FechaCita)
                .Select(c => new CitaDomain
                {
                    IdCita = c.IdCita,
                    IdCliente = c.IdCliente,
                    PedidoId = c.PedidoId,
                    FechaCita = c.FechaCita,
                    Estado = c.Estado,
                    Notas = c.Notas
                })
                .ToListAsync();
        }


        public async Task<CitaDomain> CreateAsync(CitaDomain citaDomain)
        {
            var utcFecha = DateTime.SpecifyKind(citaDomain.FechaCita, DateTimeKind.Utc);

            var cita = new Citum
            {
                IdCliente = citaDomain.IdCliente,
                FechaCita = utcFecha,
                Estado = citaDomain.Estado,
                Notas = citaDomain.Notas,
                PedidoId = citaDomain.PedidoId

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

             // SOLO si viene una fecha válida
            if (citaDomain.FechaCita != default)
            {
                cita.FechaCita = DateTime.SpecifyKind(
                    citaDomain.FechaCita,
                    DateTimeKind.Utc
                );
            }

            if (citaDomain.IdHorario.HasValue)
            {
                cita.IdHorario = citaDomain.IdHorario;
            }

            // SOLO si viene estado
            if (citaDomain.Estado.HasValue)
            {
                cita.Estado = citaDomain.Estado;
            }

            // SOLO si vienen notas
            if (!string.IsNullOrWhiteSpace(citaDomain.Notas))
            {
                cita.Notas = citaDomain.Notas;
            }

            



            //var utcFecha = DateTime.SpecifyKind(citaDomain.FechaCita, DateTimeKind.Utc);
            /*if (!string.IsNullOrWhiteSpace(citaDomain.Notas))
            {
                cita.Notas = citaDomain.Notas;
                
            }

            //ita.IdCliente = citaDomain.IdCliente;
            //cita.FechaCita = utcFecha;
            cita.FechaCita = citaDomain.FechaCita;
            cita.Estado = citaDomain.Estado;*/
            //cita.Notas = citaDomain.Notas; esta arriba en el if

            //_context.Cita.Update(cita);
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
