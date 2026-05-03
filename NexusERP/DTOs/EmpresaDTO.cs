namespace ApiNexusERP.DTOs
{
    public class EmpresaDTO
    {
        public int Id { get; set; }

        public string NombreComercial { get; set; } = null!;

        public string RazonSocial { get; set; } = null!;

        public string Cif { get; set; } = null!;

        public DateTime? FechaAlta { get; set; }

        public bool? Activo { get; set; }
    }
}
