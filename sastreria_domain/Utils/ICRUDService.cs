using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sastreria_domain.Utils
{
	public interface ICRUDService<T>
    {
        public Task<List<T>> GetAllAsync();
        public Task<T?> GetByIdAsync(int id);
        public Task<T> CreateAsync(T entity);
        public Task UpdateAsync(int id, T entity);
        public Task DeleteAsync(int id);
        public Task LogicDeleteAsync(int id);

    }

}
