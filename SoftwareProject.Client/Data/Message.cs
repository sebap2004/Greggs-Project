using System.ComponentModel.DataAnnotations;

namespace SoftwareProject.Client.Data;

/// <summary>
/// Message contains references to the Message table in the database.
/// Use this to access the columns on the Message table.
/// </summary>
public class Message
{
    [Key]
    public int message_id { get; set; }
    public int airesponse { get; set; }
    public string messagetext { get; set; }
    public DateTime timesent { get; set; }
    public int topic_id { get; set; }
}

public class MessageModel
{
    public int MessageID { get; set; }
    public int AiResponse { get; set; }
    public string MessageText { get; set; }
    public DateTime TimeSent { get; set; }
    public int TopicID { get; set; }
}

