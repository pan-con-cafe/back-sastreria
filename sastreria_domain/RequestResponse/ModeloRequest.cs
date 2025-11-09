using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sastreria_domain.RequestResponse
{
    public class ModeloRequest
    {
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public List<int>? CategoriasIds { get; set; }
        public List<string>? Imagenes { get; set; }
    }

}
