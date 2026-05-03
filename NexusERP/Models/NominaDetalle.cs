using System;
using System.Collections.Generic;

namespace NexusERP.Models;

public partial class NominaDetalle
{
    public int Id { get; set; }

    public int NominaId { get; set; }

    public int? ConceptoId { get; set; }

    public string Codigo { get; set; } = null!;

    public string ConceptoNombre { get; set; } = null!;

    public decimal Importe { get; set; }

    public int Tipo { get; set; }

    public virtual ConceptosSalariale? Concepto { get; set; }

    public virtual Nomina Nomina { get; set; } = null!;
}
