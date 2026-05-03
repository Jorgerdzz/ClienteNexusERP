using System;
using System.Collections.Generic;

namespace NexusERP.Models;

public partial class CuentasContable
{
    public int Id { get; set; }

    public int EmpresaId { get; set; }

    public string Codigo { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string? Tipo { get; set; }

    public virtual ICollection<ApuntesContable> ApuntesContables { get; set; } = new List<ApuntesContable>();

    public virtual ICollection<ConceptosSalariale> ConceptosSalariales { get; set; } = new List<ConceptosSalariale>();

    public virtual Empresa Empresa { get; set; } = null!;
}
