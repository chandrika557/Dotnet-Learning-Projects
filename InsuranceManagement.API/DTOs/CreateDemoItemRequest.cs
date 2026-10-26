namespace InsuranceManagement.API.DTOs;

/// <summary>Request body for adding an item to the in-memory demo collection.</summary>
public sealed record CreateDemoItemRequest
{
    /// <summary>Gets the item name.</summary>
    public required string Name { get; init; }
}
