using Magic.IndexedDb;
using Magic.IndexedDb.SchemaAnnotations;
using SoftwareProject.Client.Models;

namespace SoftwareProject.Client.Data;


/// <summary>
/// Topic table model for local topics.
/// </summary>
public class Topics : MagicTableTool<Topics>, IMagicTable<IndexedDBContext>
{
    
    /// <summary>
    /// Constructor for a new topics object. Assigns a new message list to avoid exceptions.
    /// </summary>
    public Topics()
    {
        Topic = new LocalTopicModel
        {
            Topic = string.Empty,
            messages = new List<LocalMessageModel>()
        };
    }

    /// <summary>
    /// Represents a static reference to the IndexedDbSet associated with the "Topics" table.
    /// </summary>
    /// <remarks>
    /// </remarks>
    public static readonly IndexedDbSet TopicSet = IndexedDBContext.Topics;

    /// <summary>
    /// Gets table name. Doing this allows rename of classes without breaking migrations.
    /// </summary>
    /// <returns>Table Name</returns>
    public string GetTableName() => "Topics";

    
    /// <summary>
    /// Not used.
    /// </summary>
    /// <returns>null</returns>
    public List<IMagicCompoundIndex>? GetCompoundIndexes() =>
        null;

    /// <summary>
    /// Creates and returns the primary key for the table.
    /// </summary>
    /// <returns>Primary key for topics table.</returns>
    public IMagicCompoundKey GetKeys() =>
        CreatePrimaryKey(x => x.Id, true);

    /// <summary>
    /// Retrieves the default database table from the IndexedDB context.
    /// </summary>
    /// <returns>Static reference to the IndexedDbSet representing the "Topics" table</returns>
    public IndexedDbSet GetDefaultDatabase() => IndexedDBContext.Topics;

    /// <summary>
    /// Represents the property for accessing the default database context instance.
    /// </summary>

    public IndexedDBContext Databases { get; } = new();

    
    /// <summary>
    /// Primary key for topic object.
    /// </summary>
    [MagicIndex]
    public long Id { get; set; }
    
    /// <summary>
    /// GUID used for interaction with web frontend.
    /// </summary>
    public string GUID { get; set; }
    
    /// <summary>
    /// Topic model storing all the data.
    /// </summary>
    public LocalTopicModel Topic { get; set; }
}