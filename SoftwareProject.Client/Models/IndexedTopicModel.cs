using System.ComponentModel.DataAnnotations;
using Magic.IndexedDb.SchemaAnnotations;

namespace SoftwareProject.Client.Models;

public class IndexedTopicModel
{
    [MagicIndex]
    public long Id { get; set; }
    
    public string GUID { get; set; }
    public string Topic { get; set; }
    public List<Message> messages { get; set; } = new();
}
