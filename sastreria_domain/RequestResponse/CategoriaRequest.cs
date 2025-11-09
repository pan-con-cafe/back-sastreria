
namespace WebSastreria.models
{
    public class CategoriaRequest
    {
        public int IdCategoria { get; set; }
        public string Nombre { get; set; } = null!;
        public bool Estado { get; set; }
        public string? Descripcion { get; set; }
        public List<int>? IdsModelos { get; set; } = new();
    }
}
