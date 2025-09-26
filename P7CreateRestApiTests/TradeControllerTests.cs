using Dot.Net.WebApi.Controllers;
using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

public class TradeControllerTests
{
    private LocalDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<LocalDbContext>()
           .UseInMemoryDatabase(databaseName: $"TradeTestDb_{Guid.NewGuid()}")
            .Options;
        return new LocalDbContext(options);
    }

    [Fact]
    public async Task Create_Should_Add_Trade()
    {
        using var context = GetDbContext();
        var repo = new TradeRepository(context);
        var controller = new TradeController(repo);

        var trade = new Trade { Account = "ACC123", Type = TradeType.BUY };

        var result = await controller.Create(trade);
        var created = Assert.IsType<CreatedAtActionResult>(result);
        var returned = Assert.IsType<Trade>(created.Value);

        Assert.Equal("ACC123", returned.Account);
    }

    [Fact]
    public async Task GetAll_Should_Return_List()
    {
        using var context = GetDbContext();
        var repo = new TradeRepository(context);
        var controller = new TradeController(repo);

        context.Trades.Add(new Trade { Account = "A1", Type = TradeType.BUY});
        context.Trades.Add(new Trade { Account = "A2", Type = TradeType.SELL});
        await context.SaveChangesAsync();

        var result = await controller.GetAll();
        var ok = Assert.IsType<OkObjectResult>(result);
        var list = Assert.IsAssignableFrom<IEnumerable<Trade>>(ok.Value);

        Assert.Equal(2, list.Count());
    }

    [Fact]
    public async Task Update_Should_Modify_Trade()
    {
        using var context = GetDbContext();
        var repo = new TradeRepository(context);
        var controller = new TradeController(repo);

        var trade = new Trade { Account = "A1", Type = TradeType.BUY};
        context.Trades.Add(trade);
        await context.SaveChangesAsync();

        trade.Type = TradeType.SELL; // mise à jour
        var result = await controller.Update(trade.TradeId, trade);

        Assert.IsType<NoContentResult>(result);
        var updated = await context.Trades.FindAsync(trade.TradeId);
        Assert.Equal("SELL", updated.Type.ToString());
    }

    [Fact]
    public async Task Delete_Should_Remove_Trade()
    {
        using var context = GetDbContext();
        var repo = new TradeRepository(context);
        var controller = new TradeController(repo);

        var trade = new Trade { Account = "A1", Type = TradeType.BUY};
        context.Trades.Add(trade);
        await context.SaveChangesAsync();

        var result = await controller.Delete(trade.TradeId);
        Assert.IsType<NoContentResult>(result);

        var deleted = await context.Trades.FindAsync(trade.TradeId);
        Assert.Null(deleted);
    }
}
