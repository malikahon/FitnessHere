using System;
using System.Collections.Generic;

namespace FitnessHere.DAL.Entities;

public partial class Class
{
    public int ClassId { get; set; }

    public string ClassName { get; set; } = null!;

    public int TrainerId { get; set; }

    public DateTime ScheduleDateTime { get; set; }

    public int? RecurrenceId { get; set; }

    public int DurationMinutes { get; set; }

    public int Capacity { get; set; }

    public decimal Price { get; set; }

    public bool IsAvailable { get; set; }

    public virtual ICollection<MemberClass> MemberClasses { get; set; } = new List<MemberClass>();

    public virtual Recurrence? Recurrence { get; set; }

    public virtual Trainer Trainer { get; set; } = null!;
}
