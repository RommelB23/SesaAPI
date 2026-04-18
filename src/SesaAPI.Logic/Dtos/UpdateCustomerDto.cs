namespace SesaAPI.Logic.Dtos
{
    public interface IUpdateCustomerDto
    {
        int Id { get; set; }
        string FullName { get; set; }
        string Identification { get; set; }
        string Email { get; set; }
    }

    public class UpdateCustomerDto : IUpdateCustomerDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string Identification { get; set; } = null!;
        public string Email { get; set; } = null!;
    }
}
