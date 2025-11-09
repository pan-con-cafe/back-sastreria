using System;
using System.Collections.Generic;

namespace sastreria_domain.entities
{
    public class ModeloDomain
    {
        public int IdModelo { get; set; }

        public string Nombre { get; set; } = null!;
        public string? Descripcion { get; set; }
        public DateTime? FechaCreacion { get; set; }

        // ✅ Imágenes asociadas
        public List<string>? Imagenes { get; set; }

        // ✅ Categorías a las que pertenece el modelo (para mostrar)
        public List<string>? Categorias { get; set; }

        // ✅ IDs de categorías seleccionadas (para crear o actualizar)
        public List<int>? CategoriasIds { get; set; }
    }
}
