namespace NexusERP.ViewModels
{
    public class DepartamentoCardViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public decimal PresupuestoAnual { get; set; }
        public decimal PresupuestoMensual { get; set; }
        public int NumeroEmpleados { get; set; }
        public decimal SalarioPromedio { get; set; }
    }
}
