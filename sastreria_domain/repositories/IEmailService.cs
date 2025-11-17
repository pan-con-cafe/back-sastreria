using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sastreria_domain.repositories
{
    public interface IEmailService
    {
        Task<bool> EnviarCorreoReservaAsync(
            string destinatario,
            string nombreCliente,
            DateTime fechaCita,
            string horario,
            string modelo
        );
    }
}
