using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sastreria_domain.entities
{
    public class CitaDomain
    {
        public int IdCita { get; set; }
        public int? IdCliente { get; set; }
        public int? PedidoId { get; set; }
        public DateTime FechaCita { get; set; }
        public bool? Estado { get; set; }
        public string? Notas { get; set; }

        public PedidoDomain? Pedido { get; set; }
        public ClienteDomain? Cliente { get; set; }
    }
}

