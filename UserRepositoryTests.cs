using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

public class UserRepositoryTests
{
    private LocalDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<LocalDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;

        return new LocalDbContext(options);
    }

    [Fact]
    public async Task FindByUserNameAsync_Should_Return_User()
    {
        using var context = GetDbContext();
        var repo = new UserRepository(context);

        context.Users.Add(new User { Username = "testuser", PasswordHash = "hash", Fullname = "Test User", Role = "USER" });
        await context.SaveChangesAsync();

        var user = await repo.FindByUserNameAsync("testuser");

        Assert.NotNull(user);
        Assert.Equal("testuser", user.Username);
    }
}
