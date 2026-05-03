namespace ApiNexusERP.DTOs
{
    public class ApunteContableDTO
    {
        public int Id { get; set; }
        public int CuentaId { get; set; }

        public string CuentaCodigo { get; set; }
        public string CuentaNombre { get; set; }

        public decimal Debe { get; set; }
        public decimal Haber { get; set; }
    }
}
