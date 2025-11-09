using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sastreria_domain.entities
{
    public class ClienteDomain
    {
        public int IdCliente { get; set; }
        public int IdTipoDocumento { get; set; }
        public string Nombre { get; set; } = null!;
        public string Apellido { get; set; } = null!;
        public string? Correo { get; set; }
        public string Telefono { get; set; } = null!;
        public string NumeroDocumento { get; set; } = null!;

    }
}

