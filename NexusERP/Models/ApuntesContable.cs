using System;
using System.Collections.Generic;

namespace NexusERP.Models;

public partial class ApuntesContable
{
    public int Id { get; set; }

    public int AsientoId { get; set; }

    public int CuentaId { get; set; }

    public decimal? Debe { get; set; }

    public decimal? Haber { get; set; }

    public virtual AsientosContable Asiento { get; set; } = null!;

    public virtual CuentasContable Cuenta { get; set; } = null!;
}
