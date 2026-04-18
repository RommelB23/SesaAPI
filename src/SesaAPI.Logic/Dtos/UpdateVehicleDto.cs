namespace SesaAPI.Logic.Dtos
{
    public interface IUpdateVehicleDto
    {
        int Id { get; set; }
        string LicensePlate { get; set; }
        string Brand { get; set; }
        string Model { get; set; }
        int Year { get; set; }
        decimal CommercialValue { get; set; }
    }

    public class UpdateVehicleDto : IUpdateVehicleDto
    {
        public int Id { get; set; }
        public string LicensePlate { get; set; } = null!;
        public string Brand { get; set; } = null!;
        public string Model { get; set; } = null!;
        public int Year { get; set; }
        public decimal CommercialValue { get; set; }
    }
}
