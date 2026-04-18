using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SesaAPI.Logic.Dtos
{
    public interface ICreatePolicyDto
    {
        int CustomerId { get; set; }
        int VehicleId { get; set; }
        String LicensePlate { get; set; }
        String Brand { get; set; }
        String Model { get; set; }
        int Year { get; set; }
        decimal CommercialValue { get; set; }
        DateTime IssueDate { get; set; }
        List<CoverageIdDto> CoveragesId { get; set; }
    }

    public class CreatePolicyDto : ICreatePolicyDto
    {
        public int CustomerId { get; set; }
        public int VehicleId { get; set; }
        public String LicensePlate { get; set; } = String.Empty;
        public String Brand { get; set; } = String.Empty;
        public String Model { get; set; } = String.Empty;
        public int Year { get; set; }
        public decimal CommercialValue { get; set; }
        public DateTime IssueDate { get; set; }
        public List<CoverageIdDto> CoveragesId { get; set; } = new();
    }
}
