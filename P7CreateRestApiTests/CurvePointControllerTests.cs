using Dot.Net.WebApi.Controllers;
using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P7CreateRestApi.Services;
using System.Threading.Tasks;
using Xunit;

public class CurvePointControllerTests
{
    private LocalDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<LocalDbContext>()
          .UseInMemoryDatabase(databaseName: $"CurveTestDb_{Guid.NewGuid()}")
            .Options;

        return new LocalDbContext(options);
    }

    [Fact]
    public async Task Create_Should_Add_CurvePoint()
    {
        using var context = GetDbContext();
        var repo = new CurvePointRepository(context);
        var controller = new CurvePointController(repo);

        var curve = new CurvePoint
        {
            CurveId = 1,
            Term = 10.5,
            Value = 99.9
        };

        var result = await controller.Create(curve);

        var created = Assert.IsType<CreatedAtActionResult>(result);
        var returned = Assert.IsType<CurvePoint>(created.Value);

        Assert.Equal(10.5, returned.Term);
    }

    [Fact]
    public async Task GetAll_Should_Return_List()
    {
        using var context = GetDbContext();
        var repo = new CurvePointRepository(context);
        var controller = new CurvePointController(repo);

        context.CurvePoints.Add(new CurvePoint { CurveId = 1, Term = 1, Value = 10 });
        context.CurvePoints.Add(new CurvePoint { CurveId = 2, Term = 2, Value = 20 });
        await context.SaveChangesAsync();

        var result = await controller.GetAll();

        var ok = Assert.IsType<OkObjectResult>(result);
        var list = Assert.IsAssignableFrom<IEnumerable<CurvePoint>>(ok.Value);

        Assert.Equal(2, list.Count());
    }

    [Fact]
    public async Task GetById_Should_Return_CurvePoint()
    {
        using var context = GetDbContext();
        var repo = new CurvePointRepository(context);
        var controller = new CurvePointController(repo);

        var curve = new CurvePoint { CurveId = 1, Term = 5, Value = 55 };
        context.CurvePoints.Add(curve);
        await context.SaveChangesAsync();

        var result = await controller.GetById(curve.Id);

        var ok = Assert.IsType<OkObjectResult>(result);
        var returned = Assert.IsType<CurvePoint>(ok.Value);

        Assert.Equal(55, returned.Value);
    }

    [Fact]
    public async Task Update_Should_Modify_CurvePoint()
    {
        using var context = GetDbContext();
        var repo = new CurvePointRepository(context);
        var controller = new CurvePointController(repo);

        var curve = new CurvePoint { CurveId = 1, Term = 5, Value = 55 };
        context.CurvePoints.Add(curve);
        await context.SaveChangesAsync();

        curve.Value = 77; // update

        var result = await controller.Update(curve.Id, curve);

        Assert.IsType<NoContentResult>(result);

        var updated = await context.CurvePoints.FindAsync(curve.Id);
        Assert.Equal(77, updated.Value);
    }

    [Fact]
    public async Task Delete_Should_Remove_CurvePoint()
    {
        using var context = GetDbContext();
        var repo = new CurvePointRepository(context);
        var controller = new CurvePointController(repo);

        var curve = new CurvePoint { CurveId = 1, Term = 5, Value = 55 };
        context.CurvePoints.Add(curve);
        await context.SaveChangesAsync();

        var result = await controller.Delete(curve.Id);
        Assert.IsType<NoContentResult>(result);

        var deleted = await context.CurvePoints.FindAsync(curve.Id);
        Assert.Null(deleted);
    }
}
