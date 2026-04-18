namespace SesaAPI.Logic.Dtos
{
    public interface ICreateVehicleDto
    {
        string LicensePlate { get; set; }
        string Brand { get; set; }
        string Model { get; set; }
        int Year { get; set; }
        decimal CommercialValue { get; set; }
    }

    public class CreateVehicleDto : ICreateVehicleDto
    {
        public string LicensePlate { get; set; } = null!;
        public string Brand { get; set; } = null!;
        public string Model { get; set; } = null!;
        public int Year { get; set; }
        public decimal CommercialValue { get; set; }
    }
}
