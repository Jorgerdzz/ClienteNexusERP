using System;
using System.Collections.Generic;

namespace NexusERP.Models;

public partial class ConceptosFijosEmpleado
{
    public int Id { get; set; }

    public int EmpresaId { get; set; }

    public int EmpleadoId { get; set; }

    public int ConceptoId { get; set; }

    public decimal ImporteFijo { get; set; }

    public bool? Activo { get; set; }

    public virtual ConceptosSalariale Concepto { get; set; } = null!;

    public virtual Empleado Empleado { get; set; } = null!;

    public virtual Empresa Empresa { get; set; } = null!;
}
