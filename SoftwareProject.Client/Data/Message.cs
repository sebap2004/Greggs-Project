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
    public bool airesponse { get; set; }
    public string messagetext { get; set; }
    public DateTime timesent { get; set; }
    public int topic_id { get; set; }
}

public class MessageDto
{
    public int MessageId { get; set; }
    public bool AiResponse { get; set; }
    public string MessageText { get; set; }
    public DateTime TimeSent { get; set; }
    public int TopicId { get; set; }
}
