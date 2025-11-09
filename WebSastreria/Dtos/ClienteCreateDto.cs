namespace WebSastreria.Dtos
{
    public class ClienteCreateDto
    {
        public int IdTipoDocumento { get; set; }
        public string NumeroDocumento { get; set; } = null!;
        public string? Correo { get; set; }
        public string Telefono { get; set; } = null!;
        public string? Nombres { get; set; } = null!;
        public string? ApellidoPaterno { get; set; }
        public string? ApellidoMaterno { get; set; }
    }

}

