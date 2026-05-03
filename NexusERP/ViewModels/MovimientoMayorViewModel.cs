namespace NexusERP.ViewModels
{
    public class MovimientoMayorViewModel
    {
        public DateTime Fecha { get; set; }
        public string AsientoNumero { get; set; }
        public string Concepto { get; set; }
        public decimal Debe { get; set; }
        public decimal Haber { get; set; }
        public decimal SaldoAcumulado { get; set; }
        public bool EsSaldoInicial { get; set; }
    }
}
