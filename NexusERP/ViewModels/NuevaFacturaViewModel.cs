namespace NexusERP.ViewModels
{
    public class NuevaFacturaViewModel
    {
        public int ClienteId { get; set; }
        public DateTime FechaEmision { get; set; }
        public string NumeroFactura { get; set; }
        public decimal PorcentajeIva { get; set; } = 21;

        // Lista de líneas que vendrán del formulario HTML dinámico
        public List<NuevaFacturaLineaViewModel> Lineas { get; set; } = new List<NuevaFacturaLineaViewModel>();
    }
}
