using System;
using System.Collections.Generic;

namespace FitnessHere.DAL.Entities;

public partial class Recurrence
{
    public int RecurrenceId { get; set; }

    public string RecurrenceType { get; set; } = null!;

    public virtual ICollection<Class> Classes { get; set; } = new List<Class>();
}
