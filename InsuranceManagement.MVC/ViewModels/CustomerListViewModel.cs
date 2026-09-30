namespace InsuranceManagement.MVC.ViewModels;

public sealed class CustomerListViewModel
{
    public IReadOnlyList<CustomerListItemViewModel> Customers { get; init; } =
        Array.Empty<CustomerListItemViewModel>();
}
