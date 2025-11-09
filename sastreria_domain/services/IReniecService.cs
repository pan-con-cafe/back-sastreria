using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sastreria_domain.dtos;


namespace sastreria_domain.services
{
    public interface IReniecService
    {
        Task<DatosReniecDto?> ObtenerDatosPorDniAsync(string dni);

    }
}
