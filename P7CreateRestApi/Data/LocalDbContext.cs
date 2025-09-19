using Microsoft.EntityFrameworkCore;
using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Controllers.Domain;
using Dot.Net.WebApi.Controllers;
using System.Security.Cryptography;

namespace Dot.Net.WebApi.Data
{
    public class LocalDbContext : DbContext
    {
        public LocalDbContext(DbContextOptions<LocalDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Trade> Trades { get; set; }
        public DbSet<Bid> Bids { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<RuleName> RuleNames { get; set; }
        public DbSet<CurvePoint> CurvePoints { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>().HasKey(u => u.Id);
            builder.Entity<Trade>().HasKey(t => t.TradeId);
            builder.Entity<Bid>().HasKey(b => b.BidId);
            builder.Entity<Rating>().HasKey(r => r.Id);
            builder.Entity<RuleName>().HasKey(r => r.Id);
            builder.Entity<CurvePoint>().HasKey(c => c.Id);
        }
  
    }
}