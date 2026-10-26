namespace InsuranceManagement.Application.Services;

/// <summary>Indicates that a requested domain resource does not exist.</summary>
/// <param name="resource">Name of the missing resource.</param>
/// <param name="key">Identifier used to locate the resource.</param>
public sealed class ResourceNotFoundException(string resource, object key)
    : Exception($"{resource} '{key}' was not found.");
