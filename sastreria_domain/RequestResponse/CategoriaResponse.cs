namespace WebSastreria.models
{

    public class CategoriaResponse
    {
        public int IdCategoria { get; set; }
        public string Nombre { get; set; } = null!;
        public string? Descripcion { get; set; }
        public bool Estado{ get; set; }

    }
}
