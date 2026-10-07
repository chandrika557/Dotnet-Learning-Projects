namespace InsuranceManagement.API.Services;

/// <summary>Indicates that an operation conflicts with existing domain data.</summary>
public sealed class ConflictException(string message) : Exception(message);
