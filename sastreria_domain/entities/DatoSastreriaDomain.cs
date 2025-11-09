using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sastreria_domain.entities
{
    public class DatoSastreriaDomain
    {
        public int IdDatos { get; set; }
        public string Nombre { get; set; } = null!;
        public string Telefono { get; set; } = null!;
        public string Direccion { get; set; } = null!;
        public string? Descripcion { get; set; }
        public string LogoSastreria { get; set; } = null!;
    }
}
