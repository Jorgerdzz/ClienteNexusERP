namespace NexusERP.Models.UI
{
    public class AlertMessage
    {
        public string Type { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public bool Toast { get; set; } = false;
    }
}
