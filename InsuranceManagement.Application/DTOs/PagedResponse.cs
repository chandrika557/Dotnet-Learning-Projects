namespace InsuranceManagement.Application.DTOs;

/// <summary>A page of results and the total number of matching records.</summary>
/// <typeparam name="T">Type of each result item.</typeparam>
/// <param name="Items">Records in this page.</param>
/// <param name="PageNumber">One-based current page number.</param>
/// <param name="PageSize">Maximum number of records requested per page.</param>
/// <param name="TotalCount">Total number of records matching the search filters.</param>
public sealed record PagedResponse<T>(
    IReadOnlyList<T> Items,
    int PageNumber,
    int PageSize,
    int TotalCount);
