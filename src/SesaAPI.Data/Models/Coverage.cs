using System;
using System.Collections.Generic;

namespace SesaAPI.Data.Models;

public partial class Coverage
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public decimal Rate { get; set; }
    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<PolicyCoverage> PolicyCoverages { get; set; } = new List<PolicyCoverage>();
}
