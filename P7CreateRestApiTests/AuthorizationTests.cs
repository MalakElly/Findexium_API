using Dot.Net.WebApi.Controllers;
using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P7CreateRestApi.Services;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

public class AuthorizationTests
{
    private LocalDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<LocalDbContext>()
            .UseInMemoryDatabase(databaseName: "AuthTestDb")
            .Options;

        return new LocalDbContext(options);
    }

    private static void SetUserRole(ControllerBase controller, string role)
    {
        var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Name, "testuser"),
            new Claim(ClaimTypes.Role, role)
        }, "mock"));

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };
    }

    [Fact]
    public async Task UserRole_Should_Be_Forbidden_On_AdminOnly_Endpoint()
    {
        using var context = GetDbContext();
        var repo = new UserRepository(context);
        var controller = new UserController(repo, new PasswordHasher());

        // Simule un USER
        SetUserRole(controller, "USER");

        var result = await controller.GetAll();

        Assert.IsType<ForbidResult>(result);
    }

    [Fact]
    public async Task AdminRole_Should_Access_AdminOnly_Endpoint()
    {
        using var context = GetDbContext();
        var repo = new UserRepository(context);
        var controller = new UserController(repo, new PasswordHasher());

        // Simule un ADMIN
        SetUserRole(controller, "ADMIN");

        var result = await controller.GetAll();

        Assert.IsType<OkObjectResult>(result);
    }
}
