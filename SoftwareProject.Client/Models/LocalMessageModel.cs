namespace SoftwareProject.Client.Models;



/// <summary>
/// Model representing a local message.
/// </summary>
[Serializable]
public class LocalMessageModel
{
    public string content { get; set; }
    public bool isUser { get; set; }
}