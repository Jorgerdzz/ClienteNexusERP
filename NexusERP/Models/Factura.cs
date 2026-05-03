using System;
using System.Collections.Generic;

namespace NexusERP.Models;

public partial class Factura
{
    public int Id { get; set; }

    public int EmpresaId { get; set; }

    public int ClienteId { get; set; }

    public string NumeroFactura { get; set; } = null!;

    public DateTime FechaEmision { get; set; }

    public bool EsEmitida { get; set; }

    public decimal BaseImponible { get; set; }

    public decimal IvaTotal { get; set; }

    public decimal TotalFactura { get; set; }

    public string Estado { get; set; } = null!;

    public int? AsientoId { get; set; }

    public virtual AsientosContable? Asiento { get; set; }

    public virtual Cliente Cliente { get; set; } = null!;

    public virtual Empresa Empresa { get; set; } = null!;

    public virtual ICollection<FacturaDetalle> FacturaDetalles { get; set; } = new List<FacturaDetalle>();
}
