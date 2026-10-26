using System.Collections.Concurrent;
using InsuranceManagement.API.DTOs;

namespace InsuranceManagement.API.Services;

/// <summary>Stores demo items in memory for the lifetime of the API process.</summary>
public sealed class DemoItemStore
{
    private readonly ConcurrentDictionary<int, DemoItemResponse> _items = new();
    private int _nextId;

    /// <summary>Gets all demo items ordered by ID.</summary>
    public IReadOnlyList<DemoItemResponse> GetAll() =>
        _items.Values.OrderBy(item => item.Id).ToArray();

    /// <summary>Gets a demo item by ID.</summary>
    public DemoItemResponse? GetById(int id) =>
        _items.TryGetValue(id, out var item) ? item : null;

    /// <summary>Creates a demo item.</summary>
    public DemoItemResponse Create(string name)
    {
        var id = Interlocked.Increment(ref _nextId);
        var item = new DemoItemResponse(id, name, DateTimeOffset.UtcNow);
        _items.TryAdd(id, item);
        return item;
    }
}
