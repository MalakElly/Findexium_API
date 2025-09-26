using Microsoft.EntityFrameworkCore;
using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Controllers.Domain;
using Dot.Net.WebApi.Controllers;

namespace Dot.Net.WebApi.Data
{
    public class LocalDbContext : DbContext
    {
        public LocalDbContext(DbContextOptions<LocalDbContext> options) : base(options) { }

        // Tables
        public DbSet<User> Users { get; set; }
        public DbSet<Trade> Trades { get; set; }
        public DbSet<Bid> Bids { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Rule> Rules { get; set; }
        public DbSet<CurvePoint> CurvePoints { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Définition explicite des clés primaires
            builder.Entity<User>().HasKey(u => u.Id);
            builder.Entity<Trade>().HasKey(t => t.TradeId);
            builder.Entity<Bid>().HasKey(b => b.Id);
            builder.Entity<Rating>().HasKey(r => r.Id);
            builder.Entity<Rule>().HasKey(r => r.Id);
            builder.Entity<CurvePoint>().HasKey(c => c.Id);
        }
    }
}