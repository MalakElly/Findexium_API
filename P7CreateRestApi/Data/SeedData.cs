using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Domain;
using System.Data;

namespace Dot.Net.WebApi.Data
{
    public static class SeedData
    {
        public static void Initialize(LocalDbContext context)
        {
         
            // TRADES
            if (!context.Trades.Any())
            {
                context.Trades.AddRange(
                    new Trade { Account = "ACC1", Type =TradeType.BUY, BuyQuantity = 10, BuyPrice = 100, TradeDate = DateTime.Now },
                    new Trade { Account = "ACC2", Type = TradeType.BUY, SellQuantity = 5, SellPrice = 200, TradeDate = DateTime.Now }
                );
            }

            // BIDS
            if (!context.Bids.Any())
            {
                context.Bids.AddRange(
                    new Bid { Account = "BID1", Type = "LIMIT", BidQuantity = 100, BidPrice = 15.5 },
                    new Bid { Account = "BID2", Type = "MARKET", AskQuantity = 50, AskPrice = 16.2 }
                );
            }

            // RATINGS
            if (!context.Ratings.Any())
            {
                context.Ratings.AddRange(
                    new Rating { MoodysRating = "AAA", SandPRating = "AA", FitchRating = "A", OrderNumber = 1 },
                    new Rating { MoodysRating = "BBB", SandPRating = "BB", FitchRating = "B", OrderNumber = 2 }
                );
            }

            // RULENAMES
            if (!context.Rules.Any())
            {
                context.Rules.AddRange(
                    new Domain.Rule{ Name = "Rule1", Description = "Test rule", Json = "{}", Template = "Temp1", SqlStr = "SELECT 1" },
                    new Domain.Rule { Name = "Rule2", Description = "Another rule", Json = "{}", Template = "Temp2", SqlStr = "SELECT 2" }
                );
            }

            // CURVE
            if (!context.CurvePoints.Any())
            {
                context.CurvePoints.AddRange(
                    new CurvePoint { Term = 1, Value = 10.5, AsOfDate = DateTime.Now },
                    new CurvePoint { Term = 2, Value = 20.7, AsOfDate = DateTime.Now }
                );
            }

            context.SaveChanges();
        }
    }
}
