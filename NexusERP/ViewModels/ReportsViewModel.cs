namespace NexusERP.ViewModels
{
    public class ReportsViewModel
    {
        public int AnioActual { get; set; }

        // 1. Gráfico de Evolución Mensual (Líneas)
        public List<decimal> IngresosMensuales { get; set; } = new List<decimal>(new decimal[12]);
        public List<decimal> GastosMensuales { get; set; } = new List<decimal>(new decimal[12]);

        // 2. Gráfico de Costes por Departamento (Donut)
        public List<string> DepartamentosNombres { get; set; } = new List<string>();
        public List<decimal> GastosPorDepartamento { get; set; } = new List<decimal>();

        // KPIs Rápidos
        public decimal TotalIngresosAnual => IngresosMensuales.Sum();
        public decimal TotalGastosAnual => GastosMensuales.Sum();
        public decimal BeneficioAnual => TotalIngresosAnual - TotalGastosAnual;
    }
}
