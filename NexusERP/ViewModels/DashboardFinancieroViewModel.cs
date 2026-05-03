namespace NexusERP.ViewModels
{
    public class DashboardFinancieroViewModel
    {
        public decimal TotalIngresos { get; set; }
        public decimal TotalGastos { get; set; }
        public decimal BeneficioNeto => TotalIngresos - TotalGastos;

        // 2. Datos para el Gráfico Circular (Gastos por Cuenta)
        public List<string> NombresCuentasGasto { get; set; } = new List<string>();
        public List<decimal> ImportesGasto { get; set; } = new List<decimal>();

        // 3. Datos para el Gráfico de Barras (Evolución Mensual)
        public List<string> Meses { get; set; } = new List<string> { "Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic" };
        public List<decimal> IngresosMensuales { get; set; } = new List<decimal>(new decimal[12]);
        public List<decimal> GastosMensuales { get; set; } = new List<decimal>(new decimal[12]);
    }
}
