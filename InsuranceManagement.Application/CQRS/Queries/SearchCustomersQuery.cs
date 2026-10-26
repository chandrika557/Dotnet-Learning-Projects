namespace InsuranceManagement.Application.CQRS.Queries;

/// <summary>Read request for filtering and paging through customers.</summary>
/// <param name="Search">Optional name, email, or phone search term.</param>
/// <param name="PageNumber">One-based page number.</param>
/// <param name="PageSize">Number of records to return, up to 100.</param>
/// <param name="SortBy">Supported sort field: name or email.</param>
/// <param name="SortDescending">Whether to sort in descending order.</param>
public sealed record SearchCustomersQuery(
    string? Search,
    int PageNumber,
    int PageSize,
    string? SortBy,
    bool SortDescending);
