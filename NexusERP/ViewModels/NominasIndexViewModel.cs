namespace NexusERP.ViewModels
{
    public class NominasIndexViewModel
    {
        public int MesSeleccionado { get; set; }
        public int AnoSeleccionado { get; set; }
        public int TotalPendientes => Empleados.Count(e => !e.EstaCalculada);
        public int TotalCalculadas => Empleados.Count(e => e.EstaCalculada);
        public List<EmpleadoNominaRowViewModel> Empleados { get; set; } = new List<EmpleadoNominaRowViewModel>();
    }
}
