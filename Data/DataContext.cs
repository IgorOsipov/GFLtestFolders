using GFLtestFolders.Models;
using Microsoft.EntityFrameworkCore;

namespace GFLtestFolders.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<FolderDirectory> FolderDirectories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<FolderDirectory>()
                .Property(c => c.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<FolderDirectory>()
                .Property(c => c.Title)
                .IsRequired();

            modelBuilder.Entity<FolderDirectory>()
                .Property(c => c.ParentId)
                .HasDefaultValue(0)
                .IsRequired();
        }
    }
}
