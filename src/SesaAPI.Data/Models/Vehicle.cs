using System;
using System.Collections.Generic;

namespace SesaAPI.Data.Models;

public partial class Vehicle
{
    public int Id { get; set; }

    public string LicensePlate { get; set; } = null!;

    public string Brand { get; set; } = null!;

    public string Model { get; set; } = null!;

    public int Year { get; set; }

    public decimal CommercialValue { get; set; }
    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<Policy> Policies { get; set; } = new List<Policy>();
}
