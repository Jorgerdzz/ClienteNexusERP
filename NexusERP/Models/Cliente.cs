using System;
using System.Collections.Generic;

namespace NexusERP.Models;

public partial class Cliente
{
    public int Id { get; set; }

    public int EmpresaId { get; set; }

    public string RazonSocial { get; set; } = null!;

    public string CifNif { get; set; } = null!;

    public string? Email { get; set; }

    public bool Activo { get; set; }

    public virtual Empresa Empresa { get; set; } = null!;

    public virtual ICollection<Factura> Facturas { get; set; } = new List<Factura>();
}
