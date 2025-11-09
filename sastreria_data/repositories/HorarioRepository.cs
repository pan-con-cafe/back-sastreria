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
    public class HorarioRepository : IHorarioRepository
    {
        private readonly _dbContext _context;

        public HorarioRepository(_dbContext context)
        {
            _context = context;
        }

        public async Task<List<HorarioDomain>> GetAllAsync()
        {
            return await _context.Horarios
                .Select(h => new HorarioDomain
                {
                    IdHorario = h.IdHorario,
                    Dia = h.Dia,
                    HoraInicio = h.HoraInicio,
                    HoraFin = h.HoraFin,
                    Estado = h.Estado
                })
                .ToListAsync();
        }

        public async Task<HorarioDomain?> GetByIdAsync(int id)
        {
            var horario = await _context.Horarios.FindAsync(id);
            if (horario == null) return null;

            return new HorarioDomain
            {
                IdHorario = horario.IdHorario,
                Dia = horario.Dia,
                HoraInicio = horario.HoraInicio,
                HoraFin = horario.HoraFin,
                Estado = horario.Estado
            };
        }

        public async Task<HorarioDomain> CreateAsync(HorarioDomain horarioDomain)
        {
            var horario = new Horario
            {
                Dia = horarioDomain.Dia,
                HoraInicio = horarioDomain.HoraInicio,
                HoraFin = horarioDomain.HoraFin,
                Estado = horarioDomain.Estado
            };

            _context.Horarios.Add(horario);
            await _context.SaveChangesAsync();

            horarioDomain.IdHorario = horario.IdHorario;
            return horarioDomain;
        }

        public async Task UpdateAsync(int id, HorarioDomain horarioDomain)
        {
            var horario = await _context.Horarios.FindAsync(id);
            if (horario == null) return;

            horario.Dia = horarioDomain.Dia;
            horario.HoraInicio = horarioDomain.HoraInicio;
            horario.HoraFin = horarioDomain.HoraFin;
            horario.Estado = horarioDomain.Estado;

            _context.Horarios.Update(horario);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var horario = await _context.Horarios.FindAsync(id);
            if (horario == null) return;

            _context.Horarios.Remove(horario);
            await _context.SaveChangesAsync();
        }
    }
}
