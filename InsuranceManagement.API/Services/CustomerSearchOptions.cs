namespace InsuranceManagement.API.Services;

/// <summary>Filtering, ordering, and paging options for customer searches.</summary>
public sealed record CustomerSearchOptions(
    string? Search,
    int PageNumber,
    int PageSize,
    string? SortBy,
    bool SortDescending);
