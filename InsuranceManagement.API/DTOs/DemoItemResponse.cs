namespace InsuranceManagement.API.DTOs;

/// <summary>Represents an item in the in-memory demo collection.</summary>
public sealed record DemoItemResponse(int Id, string Name, DateTimeOffset CreatedAt);
