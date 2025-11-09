namespace WebSastreria.Dtos
{
    public class HorarioDto
    {
        public int IdHorario { get; set; }
        public string Dia { get; set; } = null!;
        public TimeOnly HoraInicio { get; set; }
        public TimeOnly HoraFin { get; set; }
        public bool? Estado { get; set; }
    }
}

