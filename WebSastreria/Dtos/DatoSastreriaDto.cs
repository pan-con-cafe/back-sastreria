namespace WebSastreria.Dtos
{
    public class DatoSastreriaDto
    {
        public string Nombre { get; set; } = null!;
        public string Telefono { get; set; } = null!;
        public string Direccion { get; set; } = null!;
        public string? Descripcion { get; set; }
        public string LogoSastreria { get; set; } = null!;
    }
}