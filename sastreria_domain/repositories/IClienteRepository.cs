using sastreria_domain.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sastreria_domain.repositories
{
    public interface IClienteRepository
    {
        Task<List<ClienteDomain>> GetAllAsync();
        Task<ClienteDomain?> GetByIdAsync(int id);
        Task<ClienteDomain?> GetByNumeroDocumentoAsync(string numeroDocumento);
        Task<ClienteDomain> CreateAsync(ClienteDomain clienteDomain);
        Task UpdateAsync(int id, ClienteDomain clienteDomain);
        Task DeleteAsync(int id);

    }
}

