namespace ApiNexusERP.DTOs
{
    public class AsientoContableDTO
    {
        public int Id { get; set; }
        public DateTime? Fecha { get; set; }
        public string Glosa { get; set; } 
        public int? NumeroAsiento { get; set; }

        public List<ApunteContableDTO> Apuntes { get; set; } = new List<ApunteContableDTO>();
    }
}
