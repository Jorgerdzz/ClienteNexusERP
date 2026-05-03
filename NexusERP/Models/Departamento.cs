using System;
using System.Collections.Generic;

namespace NexusERP.Models;

public partial class Departamento
{
    public int Id { get; set; }

    public int EmpresaId { get; set; }

    public string Nombre { get; set; } = null!;

    public decimal PresupuestoAnual { get; set; }

    public virtual ICollection<ControlGasto> ControlGastos { get; set; } = new List<ControlGasto>();

    public virtual ICollection<Empleado> Empleados { get; set; } = new List<Empleado>();

    public virtual Empresa Empresa { get; set; } = null!;
}
