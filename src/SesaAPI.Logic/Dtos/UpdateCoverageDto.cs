namespace SesaAPI.Logic.Dtos
{
    public interface ICoverageDto
    {
        int Id { get; set; }
        string Name { get; set; }
        decimal Rate { get; set; }
    }

    public class UpdateCoverageDto : ICoverageDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Rate { get; set; }
    }
}
