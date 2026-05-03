namespace ApiNexusERP.DTOs
{
    public class FacturaDetalleDTO
    {
        public int Id { get; set; }
        public string Concepto { get; set; }
        public decimal Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal TotalLinea { get; set; }
    }
}
