using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sastreria_domain.entities
{
    public class HorarioDomain
    {
        public int IdHorario { get; set; }
        public string Dia { get; set; } = null!;
        public TimeOnly HoraInicio { get; set; }
        public TimeOnly HoraFin { get; set; }
        public bool? Estado { get; set; }
    }
}

