using System.ComponentModel.DataAnnotations;

namespace NexusERP.ViewModels
{
    public class CalcularNominaViewModel
    {
        public int EmpleadoId { get; set; }
        public int Mes { get; set; }
        public int Anio { get; set; }

        public string? EmpleadoIniciales { get; set; }
        public string? EmpleadoNombre { get; set; }
        public string? DepartamentoNombre { get; set; }

        public decimal SalarioBrutoAnual { get; set; }
        public decimal SalarioMensualSugerido { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateOnly FechaInicio { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateOnly FechaFin { get; set; }

        public decimal PorcentajeIRPF { get; set; }

        public List<ConceptoNominaItemViewModel> Conceptos { get; set; } = new List<ConceptoNominaItemViewModel>();

        public decimal SalarioBase { get; set; }
        public decimal Incentivos { get; set; }
        public decimal PlusDedicacion { get; set; }
        public decimal PlusAntiguedad { get; set; }
        public decimal PlusActividad { get; set; }
        public decimal PlusNocturnidad { get; set; }
        public decimal PlusResponsabilidad { get; set; }
        public decimal PlusConvenio { get; set; }
        public decimal PlusIdiomas { get; set; }
        public decimal HorasExtraordinarias { get; set; }
        public decimal HorasComplementarias { get; set; }
        public decimal SalarioEspecie { get; set; }

        // ==========================================
        // 2. DEVENGOS NO SALARIALES (Nuevos campos)
        // ==========================================
        public decimal IndemnizacionesSuplidos { get; set; }
        public decimal PrestacionesSS { get; set; }
        public decimal IndemnizacionesDespido { get; set; }
        public decimal PlusTransporte { get; set; }
        public decimal Dietas { get; set; }

        // ==========================================
        // 3. BASES DE COTIZACIÓN (Nuevos campos ocultos)
        // ==========================================
        public decimal BaseCotizacion_CC { get; set; }
        public decimal BaseCotizacion_CP { get; set; }
        public decimal BaseIRPF { get; set; }

        // ==========================================
        // 4. TOTALES TRABAJADOR Y EMPRESA
        // ==========================================
        public decimal TotalDevengado { get; set; }
        public decimal SS_ContingenciasComunes { get; set; }
        public decimal SS_Desempleo { get; set; }
        public decimal SS_MEI { get; set; }
        public decimal SS_Formacion { get; set; }
        public decimal RetencionIRPF { get; set; }
        public decimal TotalDeducciones { get; set; }
        public decimal LiquidoAPercibir { get; set; }

        public decimal SS_Empresa_ContingenciasComunes { get; set; }
        public decimal SS_Empresa_AccidentesTrabajo { get; set; }
        public decimal SS_Empresa_Desempleo { get; set; }
        public decimal SS_Empresa_Formacion { get; set; }
        public decimal SS_Empresa_Fogasa { get; set; }
        public decimal SS_Empresa_MEI { get; set; }
        public decimal SS_Empresa_HorasExtras { get; set; }
        public decimal SS_Empresa_Total { get; set; }
        public decimal CosteTotalEmpresa { get; set; }

    }
}
