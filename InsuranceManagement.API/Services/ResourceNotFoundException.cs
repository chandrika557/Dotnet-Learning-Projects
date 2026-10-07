namespace InsuranceManagement.API.Services;

/// <summary>Indicates that a requested domain resource does not exist.</summary>
public sealed class ResourceNotFoundException(string resource, object key)
    : Exception($"{resource} '{key}' was not found.");
