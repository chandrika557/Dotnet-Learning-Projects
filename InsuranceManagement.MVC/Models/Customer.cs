namespace InsuranceManagement.MVC.Models
{
    public class Customer //customer model class/entity class
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
    }
}