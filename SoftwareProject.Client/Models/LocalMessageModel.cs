namespace SoftwareProject.Client.Models;


[Serializable]
public class LocalMessageModel
{
    public string content { get; set; }
    public bool isUser { get; set; }
}