using System;
using System.Collections.Generic;

namespace FitnessHere.DAL.Entities;

public partial class MemberClassStatus
{
    public int StatusId { get; set; }

    public string StatusName { get; set; } = null!;

    public virtual ICollection<MemberClass> MemberClasses { get; set; } = new List<MemberClass>();
}
