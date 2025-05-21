using System;
using System.Collections.Generic;

namespace FitnessHere.DAL.Entities;

public partial class Trainer
{
    public int TrainerId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string Email { get; set; } = null!;

    public int ExperienceYears { get; set; }

    public byte[]? ProfilePicture { get; set; }

    public bool IsMale { get; set; }

    public decimal Salary { get; set; }

    public virtual ICollection<Class> Classes { get; set; } = new List<Class>();
}
