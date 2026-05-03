using Microsoft.AspNetCore.Mvc.Rendering;

namespace NexusERP.ViewModels
{
    public class MayorViewModel
    {
        public int? CuentaIdSeleccionada { get; set; }
        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }

        // Listas para la vista
        public List<SelectListItem> CuentasDisponibles { get; set; } = new List<SelectListItem>();
        public List<MovimientoMayorViewModel> Movimientos { get; set; } = new List<MovimientoMayorViewModel>();

        // Datos del resumen final
        public string NombreCuentaSeleccionada { get; set; }
        public decimal SaldoFinal { get; set; }
    }
}
