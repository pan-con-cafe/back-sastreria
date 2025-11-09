using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace sastreria_data.database.tables;

[Table("Sastre")]
public class Sastre
{
    [Key]
    public int IdSastre { get; set; }

    [StringLength(70)]
    [Unicode(false)]
    [Column("name")]

    public string Nombre { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    [Column("email")]

    public string Correo { get; set; } = null!;

    [StringLength(64)]
    [Unicode(false)]
    [Column("password")]

    public string Contrasenia { get; set; } = null!;

    [InverseProperty("IdSastreNavigation")]
    public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
}
