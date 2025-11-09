using sastreria_domain.entities;
using sastreria_domain.RequestResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSastreria.models;

namespace sastreria_domain.repositories
{
    public interface ICategoriaRepository
    {
        Task<List<CategoriaResponse>> GetAllAsync();
        Task<CategoriaResponse?> GetByIdAsync(int id);
        Task<CategoriaResponse> CreateAsync(CategoriaResponse categoria);
        Task UpdateAsync(int id, CategoriaResponse categoria);
        Task DeleteAsync(int id);
    }
}

