using System.ComponentModel.DataAnnotations;
using Magic.IndexedDb.SchemaAnnotations;

namespace SoftwareProject.Client.Models;

public class LocalTopicModel
{
    [MagicIndex]
    public long Id { get; set; }
    
    public string GUID { get; set; }
    public string Topic { get; set; }
    public List<LocalMessageModel> messages { get; set; } = new();
}
