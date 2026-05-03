using NexusERP.Models;

namespace NexusERP.ViewModels
{
    public class DashboardFacturacionViewModel
    {
        public int TotalClientes { get; set; }
        public int TotalFacturas { get; set; }
        public decimal FacturadoEsteMes { get; set; }
        public decimal PendienteDeCobro { get; set; }

        public List<Factura> UltimasFacturas { get; set; } = new List<Factura>();
    }
}
