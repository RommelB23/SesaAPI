using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SesaAPI.Logic.Dtos
{
    public interface IPolicyResponseDto
    {
        int Id { get; set; }
        string PolicyNumber { get; set; }
        string CustomerName { get; set; }
        string VehiclePlate { get; set; }
        decimal InsuredAmount { get; set; }
        decimal TotalPremium { get; set; }
        DateTime IssueDate { get; set; }
        bool IsActive { get; set; }
        List<CreateCoverageDto> Coverages { get; set; }
    }

    public class PolicyResponseDto : IPolicyResponseDto
    {
        public int Id { get; set; }
        public string PolicyNumber { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string VehiclePlate { get; set; } = string.Empty;
        public decimal InsuredAmount { get; set; }
        public decimal TotalPremium { get; set; }
        public DateTime IssueDate { get; set; }
        public bool IsActive { get; set; }
        public List<CreateCoverageDto> Coverages { get; set; } = new();
    }
}
