using Dot.Net.WebApi.Controllers;
using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

public class TradeControllerTests
{
    private LocalDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<LocalDbContext>()
            .UseInMemoryDatabase("TradeTestDb")
            .Options;

        return new LocalDbContext(options);
    }

    [Fact]
    public async Task Create_Should_Add_Trade()
    {
        using var context = GetDbContext();
        var controller = new TradeController(context);

        var trade = new Trade { Account = "ACC-001", Type = "BUY", BuyQuantity = 10, BuyPrice = 100 };

        var result = await controller.Create(trade);

        var created = Assert.IsType<CreatedAtActionResult>(result);
        var savedTrade = Assert.IsType<Trade>(created.Value);

        Assert.Equal("ACC-001", savedTrade.Account);
        Assert.Single(context.Trades); // vérifie que la BDD a bien 1 Trade
    }
}
