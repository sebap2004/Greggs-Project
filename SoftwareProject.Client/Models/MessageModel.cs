namespace SoftwareProject.Client.Models;


[Serializable]
public class Message
{
    public string content { get; set; }
    public bool isUser { get; set; }
}