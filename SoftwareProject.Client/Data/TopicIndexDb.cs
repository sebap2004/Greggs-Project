using IndexedDB.Blazor;
using Microsoft.JSInterop;
using SoftwareProject.Client.Models;

namespace SoftwareProject.Client.Data;


/// <summary>
/// Indexed Database for Local Topics
/// </summary>
public class TopicIndexDb : IndexedDb
    
{
    public TopicIndexDb(IJSRuntime jSRuntime, string name, int version) : base(jSRuntime, name, version)
    {
    }
    
    /// <summary>
    /// Set of topics
    /// </summary>
    public IndexedSet<LocalTopicModel> Topics { get; set; }
    
}