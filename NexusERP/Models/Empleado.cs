using System;
using System.Collections.Generic;

namespace NexusERP.Models;

public partial class Empleado
{
    public int Id { get; set; }

    public int EmpresaId { get; set; }

    public int DepartamentoId { get; set; }

    public string Nombre { get; set; } = null!;

    public string Apellidos { get; set; } = null!;

    public string Dni { get; set; } = null!;

    public string? EmailCorporativo { get; set; }

    public string? Telefono { get; set; }

    public DateOnly FechaNacimiento { get; set; }

    public string NumSeguridadSocial { get; set; } = null!;

    public DateOnly FechaAntiguedad { get; set; }

    public int GrupoCotizacion { get; set; }

    public decimal SalarioBrutoAnual { get; set; }

    public string? Iban { get; set; }

    public int NumeroHijos { get; set; }

    public int PorcentajeDiscapacidad { get; set; }

    public int EstadoCivil { get; set; }

    public bool Activo { get; set; }

    public string? FotoUrl { get; set; }

    public virtual ICollection<ConceptosFijosEmpleado> ConceptosFijosEmpleados { get; set; } = new List<ConceptosFijosEmpleado>();

    public virtual ICollection<ControlGasto> ControlGastos { get; set; } = new List<ControlGasto>();

    public virtual Departamento Departamento { get; set; } = null!;

    public virtual Empresa Empresa { get; set; } = null!;

    public virtual ICollection<Nomina> Nominas { get; set; } = new List<Nomina>();

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
