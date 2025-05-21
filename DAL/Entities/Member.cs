using System;
using System.Collections.Generic;

namespace FitnessHere.DAL.Entities;

public partial class Member
{
    public int MemberId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public DateOnly DateOfBirth { get; set; }

    public string Email { get; set; } = null!;

    public DateTime RegistrationDate { get; set; }

    public byte[]? ProfilePicture { get; set; }

    public bool IsDisabled { get; set; }

    public bool IsMale { get; set; }

    public virtual ICollection<MemberClass> MemberClasses { get; set; } = new List<MemberClass>();

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
