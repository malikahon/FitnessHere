namespace FitnessHere.Models
{
    public class MemberFilterViewModel
    {
        public IList<MemberFilteringViewModel> Members { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string SortColumn { get; set; }
        public bool SortDesc { get; set; }
        public string MemberFirstName { get; set; }
        public string MemberLastName { get; set; }
        public string ClassName { get; set; }
        public string TrainerName { get; set; }
        public DateTime? RegistrationDate { get; set; }
    }
}

