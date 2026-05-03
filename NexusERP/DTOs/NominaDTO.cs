namespace ApiNexusERP.DTOs
{
    public class NominaDTO
    {
        public int Id { get; set; }

        // Datos aplanados del Empleado (Para que el frontend pueda pintar el PDF sin hacer otra petición)
        public int EmpleadoId { get; set; }
        public string? NombreCompletoEmpleado { get; set; }
        public string? DniEmpleado { get; set; }

        // Periodo
        public int Mes { get; set; }
        public int Anio { get; set; }
        public DateOnly FechaInicio { get; set; }
        public DateOnly FechaFin { get; set; }
        public DateTime? FechaGeneracion { get; set; }

        // Totales Principales (Lo que ve el empleado)
        public decimal TotalDevengado { get; set; }
        public decimal TotalDeducciones { get; set; }
        public decimal LiquidoApercibir { get; set; }

        // Bases de cotización y Retenciones
        public decimal BaseCotizacionCc { get; set; }
        public decimal BaseCotizacionCp { get; set; }
        public decimal BaseIrpf { get; set; }
        public decimal PorcentajeIrpf { get; set; }

        // Aportaciones de la empresa ---
        public decimal SsEmpresaContingenciasComunes { get; set; }
        public decimal SsEmpresaAccidentesTrabajo { get; set; }
        public decimal SsEmpresaDesempleo { get; set; }
        public decimal SsEmpresaFormacion { get; set; }
        public decimal SsEmpresaFogasa { get; set; }
        public decimal SsEmpresaMei { get; set; }
        public decimal SsEmpresaHorasExtras { get; set; }
        public decimal SsEmpresaTotal { get; set; }
        public decimal? CosteTotalEmpresa { get; set; }

        // Estado (Ej: "Borrador", "Emitida", "Pagada")
        public string Estado { get; set; }

        //La lista de líneas anidada dentro de la nómina
        public List<NominaDetalleDTO> Detalles { get; set; } = new List<NominaDetalleDTO>();
    }
}

