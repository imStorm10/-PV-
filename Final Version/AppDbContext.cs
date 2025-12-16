using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public DbSet<Reading> Readings { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase("PVPrototipDB");
    }
}