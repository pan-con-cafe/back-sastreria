using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace sastreria_data.database.tables;

[Table("Horario")]
public partial class Horario
{
    [Key]
    [Column("idhorario")]
    public int IdHorario { get; set; }


    [StringLength(20)]
    [Unicode(false)]
    [Column("day")]
    public string Dia { get; set; } = null!;

    [Column("horainicio")]
    public TimeOnly HoraInicio { get; set; }

    [Column("horafin")]
    public TimeOnly HoraFin { get; set; }

    [Column("state")]
    public bool? Estado { get; set; }
}
