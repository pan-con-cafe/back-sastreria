using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace sastreria_data.database.tables;

[Table("Modelo")]
public partial class Modelo
{
    [Key]
    public int IdModelo { get; set; }

    //[Column("idmodeloimagen")]
    //public int? IdModeloImagen { get; set; }


    [StringLength(150)]
    [Unicode(false)]
    [Column("name")]
    public string Nombre { get; set; } = null!;

    [StringLength(300)]
    [Unicode(false)]
    [Column("description")]
    public string? Descripcion { get; set; }

    [Column("creationdate")]
    public DateTime? FechaCreacion { get; set; }

    //---------------------------------------------



    //[ForeignKey("IdModeloImagen")]
    //public virtual ModeloImagen? IdModeloImagenNavigation { get; set; }

    [InverseProperty("IdModeloNavigation")]
    public virtual ICollection<ModeloImagen> ModeloImagenes { get; set; } = new List<ModeloImagen>();


    [InverseProperty("IdModeloNavigation")]
    public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();

    public ICollection<Categoria> Categorias { get; set; } = new List<Categoria>();

}
