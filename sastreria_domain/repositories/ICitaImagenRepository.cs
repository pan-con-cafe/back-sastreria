using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sastreria_domain.entities;


namespace sastreria_domain.repositories
{
    public interface ICitaImagenRepository
    {
        Task<CitaImagenDomain> CreateAsync(CitaImagenDomain citaImagenDomain);
        Task<List<CitaImagenDomain>> GetByCitaAsync(int idCita);
    }
}
