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
using System.Collections.Generic;

public class AuthorizationTests
{
    private UserController GetControllerWithRole(string role)
    {
        //InMemory DB 
        var options = new DbContextOptionsBuilder<LocalDbContext>()
            .UseInMemoryDatabase("AuthTestDb_" + role) // base unique par rôle
            .Options;

        var context = new LocalDbContext(options);

        //user en DB
        context.Users.Add(new User { Username = "TestUser", Fullname = "Test", Role = role });
        context.SaveChanges();

        var repo = new UserRepository(context);
        var hasher = new PasswordHasher();

        var controller = new UserController(repo, hasher);

        //utilisateur connecté
        var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Name, "TestUser"),
            new Claim(ClaimTypes.Role, role)
        }, "mock"));

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };

        return controller;
    }

    [Fact]
    public async Task Admin_Should_Access_GetAllUsers()
    {
        var controller = GetControllerWithRole("ADMIN");

        var result = await controller.GetAll();

        Assert.IsType<OkObjectResult>(result); // ADMIN doit passer
    }

    [Fact]
    public async Task User_Should_Be_Forbidden_On_AdminOnly_Endpoint()
    {
        var controller = GetControllerWithRole("USER");

        var result = await controller.GetAll();

        Assert.IsType<ForbidResult>(result); // USER doit être bloqué
    }
}
