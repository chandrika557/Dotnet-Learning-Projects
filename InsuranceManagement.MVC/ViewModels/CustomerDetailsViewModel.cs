namespace InsuranceManagement.MVC.ViewModels;

public sealed class CustomerDetailsViewModel
{
    public int Id { get; init; }
    public string? Name { get; init; }
    public string? Email { get; init; }
    public string? PhoneNumber { get; init; }
    public string? Address { get; init; }
}
