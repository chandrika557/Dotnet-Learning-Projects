namespace InsuranceManagement.Application.Services;

/// <summary>Indicates that an operation conflicts with existing domain data.</summary>
/// <param name="message">Safe, user-facing explanation of the conflict.</param>
public sealed class ConflictException(string message) : Exception(message);
