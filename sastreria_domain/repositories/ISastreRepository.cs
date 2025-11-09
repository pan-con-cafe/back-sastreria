using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sastreria_domain.entities;

namespace sastreria_domain.repositories
{
    public interface ISastreRepository
    {
        Task<List<SastreDomain>> GetAllAsync();
        Task<SastreDomain?> GetByIdAsync(int id);
        Task<SastreDomain> CreateAsync(SastreDomain sastreDomain);
        Task UpdateAsync(int id, SastreDomain sastreDomain);
        Task DeleteAsync(int id);
    }
}
