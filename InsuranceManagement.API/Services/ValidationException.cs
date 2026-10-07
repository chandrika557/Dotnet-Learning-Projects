namespace InsuranceManagement.API.Services;

/// <summary>Represents validation errors that are not tied to an individual request field.</summary>
public sealed class ValidationException(string message) : Exception(message);
