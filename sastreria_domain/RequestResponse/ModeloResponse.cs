using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sastreria_domain.RequestResponse
{
    public class ModeloResponse
    {
        public int IdModelo { get; set; }


        public string Nombre { get; set; } = null!;
        public string? Descripcion { get; set; }
        public DateTime? FechaCreacion { get; set; }

        // -
        public List<string>? Categorias { get; set; }
        public List<string>? Imagenes { get; set; } // si lo deseas como lista
    }
}
