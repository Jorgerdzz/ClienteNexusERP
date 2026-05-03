using NexusERP.Models;

namespace NexusERP.ViewModels
{
    public class IndexDepartamentosViewModel
    {
        public int TotalDepartamentos { get; set; }
        public int TotalEmpleadosGlobal { get; set; }
        public decimal PresupuestoTotalGlobalAnual { get; set; }
        public decimal PresupuestoTotalGlobalMensual { get; set; }
        public decimal SalarioPromedioGlobalAnual { get; set; }
        public decimal SalarioPromedioGlobalMensual { get; set; }
        public List<DepartamentoCardViewModel> Departamentos { get; set; }
        

    }
}
