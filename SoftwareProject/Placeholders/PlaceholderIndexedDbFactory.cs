using IndexedDB.Blazor;

namespace SoftwareProject.Placeholders;

public class PlaceholderIndexedDbFactory : IIndexedDbFactory
{
    public Task<T> Create<T>() where T : IndexedDb
    {
        return new Task<T>(() => null!);
    }

    public Task<T> Create<T>(int version) where T : IndexedDb
    {
        return new Task<T>(() => null!);
    }

    public Task<T> Create<T>(string name) where T : IndexedDb
    {
        return new Task<T>(() => null!);
    }

    public Task<T> Create<T>(string name, int version) where T : IndexedDb
    {
        return new Task<T>(() => null!);
    }
}