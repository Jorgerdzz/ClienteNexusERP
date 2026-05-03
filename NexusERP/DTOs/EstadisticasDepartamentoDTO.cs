namespace NexusERP.DTOs
{
    public class EstadisticasDepartamentoDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public decimal PresupuestoAnual { get; set; }
        public int NumeroEmpleados { get; set; }
        public decimal SalarioPromedio { get; set; }
    }
}
