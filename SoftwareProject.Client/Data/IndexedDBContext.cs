

using Magic.IndexedDb;
using Magic.IndexedDb.Interfaces;

namespace SoftwareProject.Client.Data;

public class IndexedDBContext : IMagicRepository
{
    public static readonly IndexedDbSet Topics = new("Topics");
}