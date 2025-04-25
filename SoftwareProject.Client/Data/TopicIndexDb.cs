using IndexedDB.Blazor;
using Microsoft.JSInterop;
using SoftwareProject.Client.Models;

namespace SoftwareProject.Client.Data;

public class TopicIndexDb : IndexedDb
    
{
    public TopicIndexDb(IJSRuntime jSRuntime, string name, int version) : base(jSRuntime, name, version)
    {
    }
    
    public IndexedSet<LocalTopicModel> Topics { get; set; }
    
}