﻿using System.ComponentModel.DataAnnotations;

namespace SoftwareProject.Data;

/// <summary>
/// Account contains references to the Account table in the database.
/// Use this to access the columns on the Account table.
/// </summary>
public class Account
{
    [Key]
    public int account_id { get; set; }
    public string username { get; set; }
    public string email { get; set; }
    public string password { get; set; }
    public string role { get; set; }
}