using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace sastreria_data.database.tables;

public partial class Citum
{
    [Key]
    [Column("idcita")]
    public int IdCita { get; set; }

    [Column("idcliente")]
    public int? IdCliente { get; set; }

    [Column("citafecha")]
    public DateTime FechaCita { get; set; }

    [Column("state")]
    public bool? Estado { get; set; }

    [StringLength(200)]
    [Unicode(false)]
    [Column("notes")]
    public string? Notas { get; set; }

    [Column("PedidoId")]
    public int? PedidoId { get; set; } // FK
    public Pedido? Pedido { get; set; } // Navegación

    [ForeignKey("IdCliente")]
    [InverseProperty("Cita")]
    public virtual Cliente? IdClienteNavigation { get; set; }
}
