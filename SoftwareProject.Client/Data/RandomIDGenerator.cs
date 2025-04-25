using System.Text;

namespace SoftwareProject.Client.Data;

/// <summary>
/// Static class to generate random IDs for Local Topics.
/// </summary>
public static class RandomIDGenerator
{
    /// <summary>
    /// Generates a random ID
    /// </summary>
    /// <returns>Generated string.</returns>
    public static string GenerateRandomID()
    {
        StringBuilder builder = new StringBuilder();
        Enumerable
            .Range(65, 26)
            .Select(e => ((char)e).ToString())
            .Concat(Enumerable.Range(97, 26).Select(e => ((char)e).ToString()))
            .Concat(Enumerable.Range(0, 10).Select(e => e.ToString()))
            .OrderBy(e => Guid.NewGuid())
            .Take(11)
            .ToList().ForEach(e => builder.Append(e));
        string id = builder.ToString();
        return id;
    }
}