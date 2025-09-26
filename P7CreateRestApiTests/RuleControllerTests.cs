using Dot.Net.WebApi.Controllers;
using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

public class RuleControllerTests
{
    private LocalDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<LocalDbContext>()
            .UseInMemoryDatabase("RuleTestDb")
            .Options;
        return new LocalDbContext(options);
    }

    [Fact]
    public async Task Create_Should_Add_Rule()
    {
        using var context = GetDbContext();
        var repo = new RuleRepository(context);
        var controller = new RuleController(repo);

        var rule = new Rule { Name = "TestRule", Description = "Desc" };

        var result = await controller.Create(rule);
        var created = Assert.IsType<CreatedAtActionResult>(result);
        var returned = Assert.IsType<Rule>(created.Value);

        Assert.Equal("TestRule", returned.Name);
    }

    [Fact]
    public async Task GetAll_Should_Return_List()
    {
        using var context = GetDbContext();
        var repo = new RuleRepository(context);
        var controller = new RuleController(repo);

        context.Rules.Add(new Rule { Name = "R1" });
        context.Rules.Add(new Rule { Name = "R2" });
        await context.SaveChangesAsync();

        var result = await controller.GetAll();
        var ok = Assert.IsType<OkObjectResult>(result);
        var list = Assert.IsAssignableFrom<IEnumerable<Rule>>(ok.Value);

        Assert.Equal(2, list.Count());
    }

    [Fact]
    public async Task Update_Should_Modify_Rule()
    {
        using var context = GetDbContext();
        var repo = new RuleRepository(context);
        var controller = new RuleController(repo);

        var rule = new Rule { Name = "R1", Description = "Old" };
        context.Rules.Add(rule);
        await context.SaveChangesAsync();

        rule.Description = "New";
        var result = await controller.Update(rule.Id, rule);

        Assert.IsType<NoContentResult>(result);
        var updated = await context.Rules.FindAsync(rule.Id);
        Assert.Equal("New", updated.Description);
    }

    [Fact]
    public async Task Delete_Should_Remove_Rule()
    {
        using var context = GetDbContext();
        var repo = new RuleRepository(context);
        var controller = new RuleController(repo);

        var rule = new Rule { Name = "R1" };
        context.Rules.Add(rule);
        await context.SaveChangesAsync();

        var result = await controller.Delete(rule.Id);
        Assert.IsType<NoContentResult>(result);

        var deleted = await context.Rules.FindAsync(rule.Id);
        Assert.Null(deleted);
    }
}
