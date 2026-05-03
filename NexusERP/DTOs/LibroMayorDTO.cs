using ApiNexusERP.DTOs;

namespace NexusERP.DTOs
{
    public class LibroMayorDTO
    {
        public decimal SaldoAnterior { get; set; }
        public List<ApunteContableDTO> Movimientos { get; set; }
    }
}
