using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using sastreria_domain.entities;

namespace sastreria_domain.repositories
{
    public interface ICitaRepository
    {
        Task<List<CitaDomain>> GetAllAsync();
        Task<CitaDomain?> GetByIdAsync(int id);
        Task<CitaDomain> CreateAsync(CitaDomain citaDomain);
        Task UpdateAsync(int id, CitaDomain citaDomain);
        Task DeleteAsync(int id);
    }
}

