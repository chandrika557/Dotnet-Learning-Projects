using System.ComponentModel.DataAnnotations;

namespace InsuranceManagement.API.DTOs;

/// <summary>Request body for replacing an insurance customer's details.</summary>
public sealed record UpdateCustomerRequest
{
    /// <summary>Customer's full name.</summary>
    [Required, StringLength(150)]
    public required string Name { get; init; }

    /// <summary>Optional email address for the customer.</summary>
    [EmailAddress, StringLength(254)]
    public string? Email { get; init; }

    /// <summary>Optional phone number for the customer.</summary>
    [StringLength(32)]
    public string? PhoneNumber { get; init; }

    /// <summary>Optional mailing address for the customer.</summary>
    [StringLength(500)]
    public string? Address { get; init; }
}
