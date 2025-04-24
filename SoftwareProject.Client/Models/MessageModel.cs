namespace SoftwareProject.Client.Models;


[Serializable]
public class MessageModel
{
    public string content { get; set; }
    public bool isUser { get; set; }
}