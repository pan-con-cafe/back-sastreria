using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace sastreria_data.database.tables
{
    public partial class CitaImagen
    {
        [Key]
        [Column("IdCitaImagen")]
        public int IdCitaImagen { get; set; }

        [Unicode(false)]
        [Column("IdCita")]
        public int IdCita { get; set; }

        [Unicode(false)]
        [Column("ImageUrl")]
        public string Url { get; set; } = null!;
    }
}
