using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sastreria_domain.entities
{
    public class CitaImagenDomain
    {
        public int IdCitaImagen { get; set; }
        public int IdCita { get; set; }
        public string Url { get; set; } = null!;
    }
}
