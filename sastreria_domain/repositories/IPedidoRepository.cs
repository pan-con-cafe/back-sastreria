using sastreria_domain.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sastreria_domain.repositories
{
    public interface IPedidoRepository
    {
        Task<List<PedidoDomain>> GetAllAsync();
        Task<PedidoDomain?> GetByIdAsync(int id);
        Task<PedidoDomain> CreateAsync(PedidoDomain pedido);
        Task UpdateAsync(int id, PedidoDomain pedido);
        Task DeleteAsync(int id);
    }

}
