using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sastreria_domain.RequestResponse
{
    public class CrearCitaDesdePedidoRequest
    {
        public int IdCliente { get; set; }
        public int IdPedido { get; set; }
        public int IdHorario { get; set; }
    }
}
