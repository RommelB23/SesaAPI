using System;
using System.Collections.Generic;

namespace SesaAPI.Data.Models;

public partial class PolicyCoverage
{
    public int Id { get; set; }

    public int PolicyId { get; set; }

    public int CoverageId { get; set; }

    public virtual Coverage Coverage { get; set; } = null!;

    public virtual Policy Policy { get; set; } = null!;
}
