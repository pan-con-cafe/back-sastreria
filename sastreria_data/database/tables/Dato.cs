using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace sastreria_data.database.tables;

[Table("DatoSastreria")]
public partial class Dato
{
    [Key]
    [Column("iddatosastreria")]
    public int IdDatos { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    [Column("name")]
    public string Nombre { get; set; } = null!;

    [StringLength(13)]
    [Unicode(false)]
    [Column("phonenumber")]
    public string Telefono { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    [Column("address")]
    public string Direccion { get; set; } = null!;

    [StringLength(150)]
    [Unicode(false)]
    [Column("description")]
    public string? Descripcion { get; set; }

    [StringLength(200)]
    [Column("picture")]
    public string LogoSastreria { get; set; } = null!;
}
