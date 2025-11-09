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
    public interface IDatoSastreriaRepository
    {
        Task<List<DatoSastreriaDomain>> GetAllAsync();
        Task<DatoSastreriaDomain?> GetByIdAsync(int id);
        Task<DatoSastreriaDomain> CreateAsync(DatoSastreriaDomain dato);
        Task UpdateAsync(int id, DatoSastreriaDomain dato);
        Task DeleteAsync(int id);
    }
}
