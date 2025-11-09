using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace sastreria_data.database.tables;

[Table("Cliente")]
public partial class Cliente
{
    [Key]
    public int IdCliente { get; set; }

    [Column("idtipodocumento")]
    public int IdTipoDocumento { get; set; }

    [StringLength(70)]
    [Unicode(false)]
    [Column("name")]
    public string Nombre { get; set; } = null!;

    [StringLength(70)]
    [Unicode(false)]
    [Column("lastname")]
    public string Apellido { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    [Column("email")]
    public string? Correo { get; set; }

    [StringLength(13)]
    [Unicode(false)]
    [Column("phonenumber")]
    public string Telefono { get; set; } = null!;

    [StringLength(20)]
    [Unicode(false)]
    [Column("numerodocumento")]
    public string NumeroDocumento { get; set; } = null!;


    [InverseProperty("IdClienteNavigation")]
    public virtual ICollection<Citum> Cita { get; set; } = new List<Citum>();

    [ForeignKey("IdTipoDocumento")]
    [InverseProperty("Clientes")]
    public virtual TipoDocumento IdTipoDocumentoNavigation { get; set; } = null!;

    [InverseProperty("IdClienteNavigation")]
    public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
}
