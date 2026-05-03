namespace NexusERP.ViewModels
{
    public class ConceptoNominaItemViewModel
    {
        public int ConceptoId { get; set; }
        public string Nombre { get; set; }
        public int Tipo { get; set; } // 1: Devengo, 2: Deducción
        public decimal Importe { get; set; }
        public bool EsProrrateable { get; set; }
        public bool TributaIRPF { get; set; }
    }
}
