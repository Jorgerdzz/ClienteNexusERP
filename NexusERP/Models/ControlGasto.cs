using System;
using System.Collections.Generic;

namespace NexusERP.Models;

public partial class ControlGasto
{
    public int Id { get; set; }

    public int EmpresaId { get; set; }

    public int DepartamentoId { get; set; }

    public int EmpleadoId { get; set; }

    public int NominaId { get; set; }

    public int Mes { get; set; }

    public int Anio { get; set; }

    public decimal ImporteGasto { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public virtual Departamento Departamento { get; set; } = null!;

    public virtual Empleado Empleado { get; set; } = null!;

    public virtual Empresa Empresa { get; set; } = null!;

    public virtual Nomina Nomina { get; set; } = null!;
}
