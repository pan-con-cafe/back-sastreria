using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace sastreria_data.database.tables;

[Table("Pedido")]
public partial class Pedido
{
    [Key]
    [Column("idpedido")]
    public int IdPedido { get; set; }

    [Column("fechaentrega")]
    public DateTime? FechaEntrega { get; set; }

    [StringLength(150)]
    [Unicode(false)]
    [Column("details")]
    public string? Detalle { get; set; }

    [Column("idsastre")]
    public int? IdSastre { get; set; }

    [Column("idcliente")]
    public int? IdCliente { get; set; }

    [Column("idestado")]
    public int? IdEstado { get; set; }

    [Column("idmodelo")]
    public int? IdModelo { get; set; }

    [ForeignKey("IdCliente")]
    [InverseProperty("Pedidos")]
    public virtual Cliente? IdClienteNavigation { get; set; }

    [ForeignKey("IdEstado")]
    [InverseProperty("Pedidos")]
    public virtual Estado? IdEstadoNavigation { get; set; }

    [ForeignKey("IdModelo")]
    [InverseProperty("Pedidos")]
    public virtual Modelo? IdModeloNavigation { get; set; }

    [ForeignKey("IdSastre")]
    [InverseProperty("Pedidos")]
    public virtual Sastre? IdSastreNavigation { get; set; }
}
