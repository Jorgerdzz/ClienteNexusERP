namespace NexusERP.ViewModels
{
    public class HomeDashboardViewModel
    {
        public string NombreUsuario { get; set; }

        public decimal TotalFacturadoAnual { get; set; }
        public decimal TotalGastoSalarial { get; set; }
        public decimal Balance => TotalFacturadoAnual - TotalGastoSalarial;
        public int FacturasPendientes { get; set; }

        public bool TieneDepartamentos { get; set; }
        public bool TieneClientes { get; set; }
        public bool TieneEmpleados { get; set; }

        public bool EstaConfigurado => TieneDepartamentos && TieneClientes && TieneEmpleados;
    }
}