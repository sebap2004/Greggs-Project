

using Magic.IndexedDb;
using Magic.IndexedDb.Interfaces;

namespace SoftwareProject.Client.Data;


/// <summary>
/// DB context for the local database system.
/// </summary>
public class IndexedDBContext : IMagicRepository
{
    public static readonly IndexedDbSet Topics = new("Topics");
}