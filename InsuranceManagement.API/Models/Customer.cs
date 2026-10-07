namespace InsuranceManagement.API.Models;

/// <summary>Represents a customer stored in the insurance database.</summary>
public sealed class Customer
{
    /// <summary>Gets or sets the database-generated customer identifier.</summary>
    public int Id { get; set; }

    /// <summary>Gets or sets the customer's full name.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Gets or sets the customer's optional email address.</summary>
    public string? Email { get; set; }

    /// <summary>Gets or sets the customer's optional phone number.</summary>
    public string? PhoneNumber { get; set; }

    /// <summary>Gets or sets the customer's optional mailing address.</summary>
    public string? Address { get; set; }

    /// <summary>Gets or sets the insurance policies owned by this customer.</summary>
    public ICollection<Policy> Policies { get; set; } = new List<Policy>();
}
