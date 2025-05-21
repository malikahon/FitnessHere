using System;
using System.Collections.Generic;

namespace FitnessHere.DAL.Entities;

public partial class MembersLog
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

    public DateTime OperationDate { get; set; }

    public string Operation { get; set; } = null!;

    public string? OperationUser { get; set; }
}
