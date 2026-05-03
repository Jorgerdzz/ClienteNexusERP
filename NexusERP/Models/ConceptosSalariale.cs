using System;
using System.Collections.Generic;

namespace NexusERP.Models;

public partial class ConceptosSalariale
{
    public int Id { get; set; }

    public int EmpresaId { get; set; }

    public int? CuentaContableId { get; set; }

    public string Codigo { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public int Tipo { get; set; }

    public bool? TributaIrpf { get; set; }

    public bool? Activo { get; set; }

    public virtual ICollection<ConceptosFijosEmpleado> ConceptosFijosEmpleados { get; set; } = new List<ConceptosFijosEmpleado>();

    public virtual CuentasContable? CuentaContable { get; set; }

    public virtual Empresa Empresa { get; set; } = null!;

    public virtual ICollection<NominaDetalle> NominaDetalles { get; set; } = new List<NominaDetalle>();
}
