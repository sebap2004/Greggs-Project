using SoftwareProject.Components.Pages;

namespace SoftwareProject.Data;
using Microsoft.EntityFrameworkCore;

public class DataContext : DbContext
{

    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        
    }
    
    public DbSet<Login> Accounts { get; set; }
    
    
}