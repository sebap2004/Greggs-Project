using Magic.IndexedDb;
using Magic.IndexedDb.SchemaAnnotations;
using SoftwareProject.Client.Models;

namespace SoftwareProject.Client.Data;

public class Topics : MagicTableTool<Topics>, IMagicTable<IndexedDBContext>
{
    public Topics()
    {
        Topic = new IndexedTopicModel
        {
            Topic = string.Empty,
            messages = new List<MessageModel>()
        };
    }
    
    public static readonly IndexedDbSet TopicSet = IndexedDBContext.Topics;

    public string GetTableName() => "Topics";

    public List<IMagicCompoundIndex>? GetCompoundIndexes() =>
        null;

    
    public IMagicCompoundKey GetKeys() =>
        CreatePrimaryKey(x => x.Id, true); // Auto-incrementing primary key

    public IndexedDbSet GetDefaultDatabase() => IndexedDBContext.Topics;

    public IndexedDBContext Databases { get; } = new();

    [MagicIndex]
    public long Id { get; set; }
    
    public string GUID { get; set; }
    public IndexedTopicModel Topic { get; set; }
}