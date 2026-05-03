namespace ApiNexusERP.DTOs
{
    public class NominaDetalleDTO
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string ConceptoNombre { get; set; }
        public decimal Importe { get; set; }

        // Tipo 1 = Devengo (Suma), Tipo 2 = Deducción (Resta)
        public int Tipo { get; set; }
    }
}
