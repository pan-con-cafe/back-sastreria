using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sastreria_domain.entities;

namespace sastreria_domain.repositories
{
    public interface ITipoDocumentoRepository
    {
        Task<List<TipoDocumentoDomain>> GetAllAsync();
        Task<TipoDocumentoDomain?> GetByIdAsync(int id);
        Task<TipoDocumentoDomain> CreateAsync(TipoDocumentoDomain tipoDocumentoDomain);
        Task UpdateAsync(int id, TipoDocumentoDomain tipoDocumentoDomain);
        Task DeleteAsync(int id);
    }
}
