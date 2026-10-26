namespace InsuranceManagement.Application.Services;

/// <summary>Represents validation errors that are not tied to an individual request field.</summary>
/// <param name="message">Safe, user-facing validation explanation.</param>
public sealed class ValidationException(string message) : Exception(message);
