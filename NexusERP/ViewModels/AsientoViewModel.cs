namespace NexusERP.ViewModels
{
    public class AsientoViewModel
    {
        public int Id { get; set; }
        public string NumeroAsiento { get; set; }
        public DateTime Fecha { get; set; }
        public string Glosa { get; set; }
        public string Origen { get; set; }
        public decimal TotalDebe { get; set; }
        public decimal TotalHaber { get; set; }
        public bool Cuadrado => TotalDebe == TotalHaber;
        public List<ApunteViewModel> Apuntes { get; set; } = new List<ApunteViewModel>();
    }
}
