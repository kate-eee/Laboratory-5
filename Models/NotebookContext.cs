using Lab5.Models;
using Microsoft.EntityFrameworkCore;

namespace Lab5.Models;

public class NotebookContext : DbContext
{
    public DbSet<NotebookEntry> Notebook { get; set; }
    
    public NotebookContext(DbContextOptions<NotebookContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<NotebookEntry>()
            // .ToTable("Archive")
            .HasKey(e => e.Id);
    }
}