using NexusERP.Models;

namespace NexusERP.ViewModels
{
    public class MisNominasViewModel
    {
        public int MesSeleccionado { get; set; }
        public int AnoSeleccionado { get; set; }
        public Nomina? NominaActual { get; set; }
    }
}
