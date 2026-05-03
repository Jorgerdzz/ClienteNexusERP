namespace ApiNexusERP.DTOs
{
    public class FacturaDTO
    {
        public int Id { get; set; }

        public int ClienteId { get; set; }
        public string? ClienteRazonSocial { get; set; }

        public string NumeroFactura { get; set; }
        public DateTime FechaEmision { get; set; }
        public bool EsEmitida { get; set; }

        public decimal BaseImponible { get; set; }
        public decimal IvaTotal { get; set; }
        public decimal TotalFactura { get; set; }
        public string Estado { get; set; } // "Pendiente", "Pagada"

        public List<FacturaDetalleDTO> Detalles { get; set; } = new List<FacturaDetalleDTO>();
    }
}
