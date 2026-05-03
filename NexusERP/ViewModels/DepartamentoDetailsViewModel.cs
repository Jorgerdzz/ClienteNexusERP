using NexusERP.Models;

namespace NexusERP.ViewModels
{
    public class DepartamentoDetailsViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public decimal PresupuestoAnual { get; set; }
        public decimal PresupuestoMensual { get; set; }
        public int NumeroEmpleados { get; set; }
        public decimal SalarioPromedioAnual { get; set; }
        public decimal SalarioPromedioMensual { get; set; }
        public List<Empleado> Empleados { get; set; } = new List<Empleado>();
    }
}
