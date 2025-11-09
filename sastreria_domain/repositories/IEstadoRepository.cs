using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sastreria_domain.entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace sastreria_domain.repositories
{
    public interface IEstadoRepository
    {
        Task<List<EstadoDomain>> GetAllAsync();
        Task<EstadoDomain?> GetByIdAsync(int id);
        Task<EstadoDomain> CreateAsync(EstadoDomain estado);
        Task UpdateAsync(int id, EstadoDomain estado);
        Task DeleteAsync(int id);
    }
}
