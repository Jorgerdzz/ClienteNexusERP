namespace ApiNexusERP.DTOs
{
    public class EmpleadoDTO
    {
        public int Id { get; set; }
        public int DepartamentoId { get; set; }
        public string? NombreDepartamento { get; set; } // Lo extraeremos con AutoMapper

        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Dni { get; set; }
        public string? EmailCorporativo { get; set; }
        public string? Telefono { get; set; }
        public decimal SalarioBrutoAnual { get; set; }

        // Aquí viene la magia de la seguridad
        public string? Iban { get; set; } // Se usa cuando hacemos POST/PUT para recibir el dato real
        public string? IbanEnmascarado { get; set; } // Se usa en los GET para mostrar "**** **** **** 1234"

        public bool Activo { get; set; }
    }
}
