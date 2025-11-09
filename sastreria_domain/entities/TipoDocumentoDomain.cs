using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sastreria_domain.entities
{
    public class TipoDocumentoDomain
    {
        public int IdTipoDocumento { get; set; }
        public string Nombre { get; set; } = null!;
    }
}
