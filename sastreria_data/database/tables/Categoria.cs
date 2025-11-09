using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace sastreria_data.database.tables;

[Table("Categoria")]
public partial class Categoria
{
    [Key]
    [Column("id")]
    public int IdCategoria { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    [Column("name")]
    public string Nombre { get; set; } = null!;

    [StringLength(150)]
    [Unicode(false)]
    [Column("description")]
    public string Descripcion { get; set; } = null!;

    [Column("estado")]
    public bool Estado { get; set; }

    public ICollection<Modelo> Modelos { get; set; } = new List<Modelo>();
}
