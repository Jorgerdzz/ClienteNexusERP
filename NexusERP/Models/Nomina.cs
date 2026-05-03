using System;
using System.Collections.Generic;

namespace NexusERP.Models;

public partial class Nomina
{
    public int Id { get; set; }

    public int EmpresaId { get; set; }

    public int EmpleadoId { get; set; }

    public int? AsientoId { get; set; }

    public int Mes { get; set; }

    public int Anio { get; set; }

    public DateOnly FechaInicio { get; set; }

    public DateOnly FechaFin { get; set; }

    public decimal TotalDevengado { get; set; }

    public decimal TotalDeducciones { get; set; }

    public decimal LiquidoApercibir { get; set; }

    public decimal BaseCotizacionCc { get; set; }

    public decimal BaseCotizacionCp { get; set; }

    public decimal BaseIrpf { get; set; }

    public decimal PorcentajeIrpf { get; set; }

    public decimal SsEmpresaContingenciasComunes { get; set; }

    public decimal SsEmpresaAccidentesTrabajo { get; set; }

    public decimal SsEmpresaDesempleo { get; set; }

    public decimal SsEmpresaFormacion { get; set; }

    public decimal SsEmpresaFogasa { get; set; }

    public decimal SsEmpresaTotal { get; set; }

    public decimal? CosteTotalEmpresa { get; set; }

    public DateTime? FechaGeneracion { get; set; }

    public decimal SsEmpresaMei { get; set; }

    public decimal SsEmpresaHorasExtras { get; set; }

    public string Estado { get; set; } = null!;

    public virtual AsientosContable? Asiento { get; set; }

    public virtual ICollection<ControlGasto> ControlGastos { get; set; } = new List<ControlGasto>();

    public virtual Empleado Empleado { get; set; } = null!;

    public virtual Empresa Empresa { get; set; } = null!;

    public virtual ICollection<NominaDetalle> NominaDetalles { get; set; } = new List<NominaDetalle>();
}
