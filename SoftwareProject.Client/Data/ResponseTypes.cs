namespace SoftwareProject.Client.Data;

/// <summary>
/// Enum to determine how the AI will respond.
/// </summary>
public enum ResponseTypes
{
    /// <summary>
    /// Will prompt a long and detailed response
    /// </summary>
    Long,
    /// <summary>
    /// Will prompt a short and concise response.
    /// </summary>
    Short,
    /// <summary>
    /// Will prompt a formal and polite response.
    /// </summary>
    Formal
}