using Dot.Net.WebApi.Controllers;
using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;
using System.Threading.Tasks;
using System.Collections.Generic;
using Dot.Net.WebApi.Controllers.Domain;

public class RatingControllerTests
{
    private LocalDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<LocalDbContext>()
            .UseInMemoryDatabase(databaseName: $"RatingTestDb_{System.Guid.NewGuid()}")
            .Options;

        return new LocalDbContext(options);
    }

    [Fact]
    public async Task Create_Then_GetById_Should_Return_Rating()
    {
        using var context = GetDbContext();
        var repo = new RatingRepository(context);
        var controller = new RatingController(repo);

        var rating = new Rating
        {
            MoodysRating = "AAA",
            SandPRating = "AA+",
            FitchRating = "AA",
            OrderNumber = 1
        };

        var result = await controller.Create(rating);
        var created = Assert.IsType<CreatedAtActionResult>(result);
        var createdRating = Assert.IsType<Rating>(created.Value);

        Assert.Equal("AAA", createdRating.MoodysRating);

        var getResult = await controller.GetById(createdRating.Id);
        var ok = Assert.IsType<OkObjectResult>(getResult);
        var retrieved = Assert.IsType<Rating>(ok.Value);

        Assert.Equal("AAA", retrieved.MoodysRating);
    }

    [Fact]
    public async Task GetAll_Should_Return_List()
    {
        using var context = GetDbContext();
        var repo = new RatingRepository(context);
        var controller = new RatingController(repo);

        context.Ratings.Add(new Rating { MoodysRating = "BBB", OrderNumber = 1 });
        context.Ratings.Add(new Rating { MoodysRating = "CCC", OrderNumber = 2 });
        await context.SaveChangesAsync();

        var result = await controller.GetAll();
        var ok = Assert.IsType<OkObjectResult>(result);
        var list = Assert.IsAssignableFrom<IEnumerable<Rating>>(ok.Value);

        Assert.Equal(2, System.Linq.Enumerable.Count(list));
    }

    [Fact]
    public async Task Update_Should_Modify_Existing_Rating()
    {
        using var context = GetDbContext();
        var repo = new RatingRepository(context);
        var controller = new RatingController(repo);

        var rating = new Rating { MoodysRating = "BBB", OrderNumber = 2 };
        context.Ratings.Add(rating);
        await context.SaveChangesAsync();

        rating.MoodysRating = "AAA";

        var result = await controller.Update(rating.Id, rating);
        Assert.IsType<NoContentResult>(result);

        var updated = await context.Ratings.FindAsync(rating.Id);
        Assert.Equal("AAA", updated.MoodysRating);
    }

    [Fact]
    public async Task Delete_Should_Remove_Rating()
    {
        using var context = GetDbContext();
        var repo = new RatingRepository(context);
        var controller = new RatingController(repo);

        var rating = new Rating { MoodysRating = "CCC", OrderNumber = 3 };
        context.Ratings.Add(rating);
        await context.SaveChangesAsync();

        var result = await controller.Delete(rating.Id);
        Assert.IsType<NoContentResult>(result);

        var deleted = await context.Ratings.FindAsync(rating.Id);
        Assert.Null(deleted);
    }
}
