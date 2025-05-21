using System;
using System.Collections.Generic;

namespace FitnessHere.DAL.Entities;

public partial class Transaction
{
    public int TransactionId { get; set; }

    public int MemberId { get; set; }

    public decimal Amount { get; set; }

    public DateTime TransactionDate { get; set; }

    public virtual Member Member { get; set; } = null!;
}
