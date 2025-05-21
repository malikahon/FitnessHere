using System.ComponentModel;

namespace FitnessHere.Models
{
    public class MemberFilteringViewModel
    {
            public int MemberID { get; set; }

            [DisplayName("First Name")]
            public string FirstName { get; set; }

            [DisplayName("Last Name")]
            public string LastName { get; set; }

            [DisplayName("Phone Number")]
            public string PhoneNumber { get; set; }

            [DisplayName("Date of Birth")]
            public DateTime DateOfBirth { get; set; }

            [DisplayName("Email Address")]
            public string Email { get; set; }

            [DisplayName("Registration Date")]
            public DateTime RegistrationDate { get; set; } = DateTime.Now;

            public byte[]? ProfilePicture { get; set; }

            [DisplayName("Disabled")]
            public bool IsDisabled { get; set; } = false;

            [DisplayName("Male")]
            public bool IsMale { get; set; } = false;

        public IList<string> MemberClasses { get; set; }

        public IList<string> Trainers { get; set;  }
    }
}
