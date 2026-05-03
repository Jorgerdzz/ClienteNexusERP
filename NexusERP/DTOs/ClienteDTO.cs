namespace ApiNexusERP.DTOs
{
    public class ClienteDTO
    {
        public int Id { get; set; }

        public int EmpresaId { get; set; }

        public string RazonSocial { get; set; } = null!;

        public string CifNif { get; set; } = null!;

        public string? Email { get; set; }

        public bool Activo { get; set; }
    }
}
