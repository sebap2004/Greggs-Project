using System.ComponentModel.DataAnnotations;

namespace SoftwareProject.Client.Data;

/// <summary>
/// Topic contains references to the Topic table in the database.
/// Use this to access the columns on the Topic table.
/// </summary>
public class Topic
{
    [Key]
    public int topic_id { get; set; }
    public string topicname { get; set; }
    public int account_id { get; set; }
}