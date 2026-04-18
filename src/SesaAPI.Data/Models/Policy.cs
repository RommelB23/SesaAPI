using System;
using System.Collections.Generic;

namespace SesaAPI.Data.Models;

public partial class Policy
{
    public int Id { get; set; }

    public string PolicyNumber { get; set; } = null!;

    public int CustomerId { get; set; }

    public int VehicleId { get; set; }

    public DateTime IssueDate { get; set; }

    public decimal InsuredAmount { get; set; }

    public decimal TotalPremium { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<PolicyCoverage> PolicyCoverages { get; set; } = new List<PolicyCoverage>();

    public virtual Vehicle Vehicle { get; set; } = null!;
}
