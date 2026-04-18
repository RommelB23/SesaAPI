using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SesaAPI.Logic.Dtos
{
    public interface IUpdatePolicyDto
    {
        int Id { get; set; }

        List<CoverageIdDto> CoveragesId { get; set; }
    }

    public class UpdatePolicyDto : IUpdatePolicyDto
    {
        public int Id { get; set; }
        public List<CoverageIdDto> CoveragesId { get; set; } = new();
    }
}
