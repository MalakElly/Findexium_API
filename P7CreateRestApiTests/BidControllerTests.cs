using Dot.Net.WebApi.Controllers;
using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

public class BidControllerTests
{
    private LocalDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<LocalDbContext>()
             .UseInMemoryDatabase(databaseName: $"BidTestDb_{Guid.NewGuid()}")
            .Options;
        return new LocalDbContext(options);
    }

    [Fact]
    public async Task Create_Should_Add_Bid()
    {
        using var context = GetDbContext();
        var repo = new BidRepository(context);
        var controller = new BidController(repo);

        var bid = new Bid { Account = "BID1", Type = "BUY" };

        var result = await controller.Create(bid);
        var created = Assert.IsType<CreatedAtActionResult>(result);
        var returned = Assert.IsType<Bid>(created.Value);

        Assert.Equal("BID1", returned.Account);
    }

    [Fact]
    public async Task GetAll_Should_Return_List()
    {
        using var context = GetDbContext();
        var repo = new BidRepository(context);
        var controller = new BidController(repo);

        context.Bids.Add(new Bid { Account = "B1", Type = "BUY" });
        context.Bids.Add(new Bid { Account = "B2", Type = "SELL" });
        await context.SaveChangesAsync();

        var result = await controller.GetAll();
        var ok = Assert.IsType<OkObjectResult>(result);
        var list = Assert.IsAssignableFrom<IEnumerable<Bid>>(ok.Value);

        Assert.Equal(2, list.Count());
    }

    [Fact]
    public async Task Update_Should_Modify_Bid()
    {
        using var context = GetDbContext();
        var repo = new BidRepository(context);
        var controller = new BidController(repo);

        var bid = new Bid { Account = "B1", Type = "BUY" };
        context.Bids.Add(bid);
        await context.SaveChangesAsync();

        bid.Type = "SELL";
        var result = await controller.Update(bid.Id, bid);

        Assert.IsType<NoContentResult>(result);
        var updated = await context.Bids.FindAsync(bid.Id);
        Assert.Equal("SELL", updated.Type);
    }

    [Fact]
    public async Task Delete_Should_Remove_Bid()
    {
        using var context = GetDbContext();
        var repo = new BidRepository(context);
        var controller = new BidController(repo);

        var bid = new Bid { Account = "B1", Type = "BUY" };
        context.Bids.Add(bid);
        await context.SaveChangesAsync();

        var result = await controller.Delete(bid.Id);
        Assert.IsType<NoContentResult>(result);

        var deleted = await context.Bids.FindAsync(bid.Id);
        Assert.Null(deleted);
    }
}
