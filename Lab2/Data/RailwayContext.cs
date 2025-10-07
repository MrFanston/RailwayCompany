using Lab2.Models;
using Microsoft.EntityFrameworkCore;

namespace Lab2.Data
{
    public class RailwayContext : DbContext
    {
        public RailwayContext(DbContextOptions<RailwayContext> options) : base(options) { }

        public DbSet<Train> Trains { get; set; }
        public DbSet<Carriage> Carriages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // один поезд → много вагонов
            modelBuilder.Entity<Carriage>()
                .HasOne(c => c.Train)
                .WithMany(t => t.Carriages)
                .HasForeignKey(c => c.TrainId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
