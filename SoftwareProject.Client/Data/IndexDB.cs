using IndexedDB.Blazor;
using Microsoft.JSInterop;

namespace SoftwareProject.Client.Data;

public class IndexDB : IndexedDb
    
{
    public IndexDB(IJSRuntime jSRuntime, string name, int version) : base(jSRuntime, name, version)
    {
    }
    
    
}