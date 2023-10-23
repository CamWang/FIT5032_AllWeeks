namespace EasyImagery.Models
{
    public class TimeslotViewModel
    {
        public List<Timeslot> Timeslots { get; set; }
        public int ClinicId { get; set; }
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
        public string SearchTerm { get; set; }
        public string SortOrder { get; set; }
    }
}
