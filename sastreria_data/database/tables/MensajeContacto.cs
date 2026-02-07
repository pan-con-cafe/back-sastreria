using System.ComponentModel.DataAnnotations.Schema;

[Table("MensajeContacto")]
public class MensajeContacto
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Asunto { get; set; } = null!;

    public string Mensaje { get; set; } = null!;

    public DateTime Fecha { get; set; } = DateTime.Now;

    public bool Leido { get; set; } = false;
}
