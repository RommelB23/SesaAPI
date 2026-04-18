using System;
using System.Collections.Generic;

namespace SesaAPI.Data.Models;

public partial class Customer
{
    public int Id { get; set; }

    public string FullName { get; set; } = null!;

    public string Identification { get; set; } = null!;

    public string Email { get; set; } = null!;
    public bool IsActive { get; set; } 

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<Policy> Policies { get; set; } = new List<Policy>();
}
