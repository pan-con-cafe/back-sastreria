using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace sastreria_data.database.tables;

[Table("ModeloImagen")]
public partial class ModeloImagen
{
    [Key]
    public int IdModeloImagen { get; set; }

    [Column("url")]
    [StringLength(500)]
    public string Url { get; set; } = null!;

    [Column("idmodelo")]
    public int IdModelo { get; set; }

    [ForeignKey("IdModelo")]
    [InverseProperty("ModeloImagenes")]
    public virtual Modelo IdModeloNavigation { get; set; } = null!;
}
