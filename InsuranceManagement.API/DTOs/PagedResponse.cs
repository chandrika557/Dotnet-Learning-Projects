namespace InsuranceManagement.API.DTOs;

/// <summary>A page of results and the total number of matching records.</summary>
public sealed record PagedResponse<T>(
    IReadOnlyList<T> Items,
    int PageNumber,
    int PageSize,
    int TotalCount);
