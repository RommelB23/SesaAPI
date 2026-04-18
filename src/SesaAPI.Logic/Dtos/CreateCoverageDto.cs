namespace SesaAPI.Logic.Dtos
{
    public interface ICreateCoverageDto
    {
        string Name { get; set; }
        decimal Rate { get; set; }
    }

    public class CreateCoverageDto : ICreateCoverageDto
    {
        public string Name { get; set; } = null!;
        public decimal Rate { get; set; }
    }
}
