using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sastreria_domain.entities;

namespace sastreria_domain.repositories
{
    public interface IHorarioRepository
    {
        Task<List<HorarioDomain>> GetAllAsync();
        Task<HorarioDomain?> GetByIdAsync(int id);
        Task<HorarioDomain> CreateAsync(HorarioDomain horario);
        Task UpdateAsync(int id, HorarioDomain horario);
        Task DeleteAsync(int id);
    }
}
