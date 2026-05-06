namespace ApiNexusERP.DTOs
{
    public class EmpleadoDTO
    {
        // --- Campos Clave ---
        public int Id { get; set; }
        public int EmpresaId { get; set; }
        public int DepartamentoId { get; set; }
        public string? NombreDepartamento { get; set; } // Extraído con AutoMapper para las Vistas

        // --- Datos Personales ---
        public string Nombre { get; set; } = null!;
        public string Apellidos { get; set; } = null!;
        public string Dni { get; set; } = null!;
        public string? EmailCorporativo { get; set; }
        public string? Telefono { get; set; }
        public DateOnly FechaNacimiento { get; set; }
        public int EstadoCivil { get; set; } // int, igual que en tu NuGet
        public int NumeroHijos { get; set; }
        public int PorcentajeDiscapacidad { get; set; }
        public string? FotoUrl { get; set; }

        // --- Datos Laborales y Financieros ---
        public string NumSeguridadSocial { get; set; } = null!;
        public DateOnly FechaAntiguedad { get; set; }
        public int GrupoCotizacion { get; set; } // int, igual que en tu NuGet
        public decimal SalarioBrutoAnual { get; set; }

        public string? Iban { get; set; }
        public string? IbanEnmascarado { get; set; } // Para mostrar "**** **** **** 1234"

        public bool Activo { get; set; }
    }
}
