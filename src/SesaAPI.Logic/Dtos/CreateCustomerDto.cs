namespace SesaAPI.Logic.Dtos
{
    public interface ICreateCustomerDto
    {
        string FullName { get; set; }
        string Identification { get; set; }
        string Email { get; set; }
    }

    public class CreateCustomerDto : ICreateCustomerDto
    {
        public string FullName { get; set; } = null!;
        public string Identification { get; set; } = null!;
        public string Email { get; set; } = null!;
    }
}
