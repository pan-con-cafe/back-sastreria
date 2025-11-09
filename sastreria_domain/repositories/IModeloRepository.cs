using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sastreria_domain.services;
using sastreria_domain.entities;

namespace sastreria_domain.repositories
{
    public interface IModeloRepository
    {
        Task<List<ModeloDomain>> GetAllAsync();
        Task<ModeloDomain?> GetByIdAsync(int id);
        Task<ModeloDomain> CreateAsync(ModeloDomain modelo);
        Task UpdateAsync(int id, ModeloDomain modelo);
        Task DeleteAsync(int id);
        Task<IEnumerable<ModeloDomain>> GetByCategoriaAsync(int idCategoria);


    }

}
