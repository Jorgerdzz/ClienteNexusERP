namespace NexusERP.DTOs
{
    public class DashboardMetricsDto
    {
        public decimal TotalFacturadoAnual { get; set; }
        public decimal TotalGastoSalarial { get; set; }
        public int FacturasPendientes { get; set; }

        public bool TieneDepartamentos { get; set; }
        public bool TieneClientes { get; set; }
        public bool TieneEmpleados { get; set; }
    }
}