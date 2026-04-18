using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SesaAPI.Data.Models;

namespace SesaAPI.Data.Context;

public partial class SesaDBContext : DbContext
{
    public SesaDBContext()
    {
    }

    public SesaDBContext(DbContextOptions<SesaDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Coverage> Coverages { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Policy> Policies { get; set; }

    public virtual DbSet<PolicyCoverage> PolicyCoverages { get; set; }

    public virtual DbSet<Vehicle> Vehicles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SesaDBContext).Assembly);
    }
}
