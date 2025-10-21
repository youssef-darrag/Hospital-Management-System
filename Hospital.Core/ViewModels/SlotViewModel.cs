namespace Hospital.Core.ViewModels
{
    public class SlotViewModel
    {
        public TimeOnly Start { get; set; }
        public TimeOnly End { get; set; }
        public bool IsBooked { get; set; }
    }
}
