namespace Assesment.Models
{
    public class InputModel
    {
        public string InputText { get; set; }
        public List<string> DetectedNumbers { get; set; } = new List<string>();
    }
}
