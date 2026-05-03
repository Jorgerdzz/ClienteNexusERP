using System;
using System.Collections.Generic;

namespace NexusERP.Models;

public partial class Empresa
{
    public int Id { get; set; }

    public string NombreComercial { get; set; } = null!;

    public string RazonSocial { get; set; } = null!;

    public string Cif { get; set; } = null!;

    public DateTime? FechaAlta { get; set; }

    public bool? Activo { get; set; }

    public virtual ICollection<AsientosContable> AsientosContables { get; set; } = new List<AsientosContable>();

    public virtual ICollection<Cliente> Clientes { get; set; } = new List<Cliente>();

    public virtual ICollection<ConceptosFijosEmpleado> ConceptosFijosEmpleados { get; set; } = new List<ConceptosFijosEmpleado>();

    public virtual ICollection<ConceptosSalariale> ConceptosSalariales { get; set; } = new List<ConceptosSalariale>();

    public virtual ICollection<ControlGasto> ControlGastos { get; set; } = new List<ControlGasto>();

    public virtual ICollection<CuentasContable> CuentasContables { get; set; } = new List<CuentasContable>();

    public virtual ICollection<Departamento> Departamentos { get; set; } = new List<Departamento>();

    public virtual ICollection<Empleado> Empleados { get; set; } = new List<Empleado>();

    public virtual ICollection<Factura> Facturas { get; set; } = new List<Factura>();

    public virtual ICollection<Nomina> Nominas { get; set; } = new List<Nomina>();

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
