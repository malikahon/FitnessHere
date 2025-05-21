using System;
using System.Collections.Generic;

namespace FitnessHere.DAL.Entities;

public partial class MemberClass
{
    public int MemberClassId { get; set; }

    public int MemberId { get; set; }

    public int ClassId { get; set; }

    public DateTime JoinDate { get; set; }

    public int MemberClassStatusId { get; set; }

    public virtual Class Class { get; set; } = null!;

    public virtual Member Member { get; set; } = null!;

    public virtual MemberClassStatus MemberClassStatus { get; set; } = null!;
}
