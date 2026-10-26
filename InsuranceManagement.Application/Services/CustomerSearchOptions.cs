namespace InsuranceManagement.Application.Services;

/// <summary>Filtering, ordering, and paging options for customer searches.</summary>
/// <param name="Search">Optional customer-name, email, or phone search text.</param>
/// <param name="PageNumber">One-based page number.</param>
/// <param name="PageSize">Number of records in a page, up to 100.</param>
/// <param name="SortBy">Supported field name or email.</param>
/// <param name="SortDescending">Whether to sort in descending order.</param>
public sealed record CustomerSearchOptions(
    string? Search,
    int PageNumber,
    int PageSize,
    string? SortBy,
    bool SortDescending);
