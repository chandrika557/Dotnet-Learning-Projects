namespace InsuranceManagement.API.DTOs;

/// <summary>Customer details returned by the insurance management API.</summary>
public sealed record CustomerResponse(
    int Id,
    string Name,
    string? Email,
    string? PhoneNumber,
    string? Address);
