using System;
using System.Collections.Generic;

namespace NexusERP.Models;

public partial class AsientosContable
{
    public int Id { get; set; }

    public int EmpresaId { get; set; }

    public DateTime? Fecha { get; set; }

    public string Glosa { get; set; } = null!;

    public int? NumeroAsiento { get; set; }

    public virtual ICollection<ApuntesContable> ApuntesContables { get; set; } = new List<ApuntesContable>();

    public virtual Empresa Empresa { get; set; } = null!;

    public virtual ICollection<Factura> Facturas { get; set; } = new List<Factura>();

    public virtual ICollection<Nomina> Nominas { get; set; } = new List<Nomina>();
}
