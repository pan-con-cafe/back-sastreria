using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace sastreria_data.database.tables;

[Table("TipoDocumento")]
public partial class TipoDocumento
{
    [Key]
    public int IdTipoDocumento { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    [Column("name")]
    public string Nombre { get; set; } = null!;

    [InverseProperty("IdTipoDocumentoNavigation")]
    public virtual ICollection<Cliente> Clientes { get; set; } = new List<Cliente>();
}
