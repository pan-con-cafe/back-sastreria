using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sastreria_domain.entities
{
    public class PedidoDomain
    {
        public int IdPedido { get; set; }
        public DateTime? FechaEntrega { get; set; }
        public string? Detalle { get; set; }
        public int? IdSastre { get; set; }
        public int? IdCliente { get; set; }
        public int? IdEstado { get; set; }
        public int? IdModelo { get; set; }
    }

}
