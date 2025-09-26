using Dot.Net.WebApi.Controllers;
using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using Dot.Net.WebApi.Models;
using Dot.Net.WebApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using P7CreateRestApi.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

public class LoginControllerTests
{
    private LocalDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<LocalDbContext>()
            .UseInMemoryDatabase(databaseName: $"LoginTestDb_{System.Guid.NewGuid()}")
            .Options;

        return new LocalDbContext(options);
    }

    private IConfiguration GetConfiguration()
    {
        var inMemorySettings = new Dictionary<string, string>
        {
            {"Jwt:Key", "ma_cle_ultra_secrete_123456"},
            {"Jwt:Issuer", "Findexium"},
            {"Jwt:Audience", "FindexiumClients"}
        };

        return new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();
    }

    [Fact]
    public async Task Login_WithValidCredentials_ShouldReturnToken()
    {
        using var context = GetDbContext();
        var repo = new UserRepository(context);
        var hasher = new PasswordHasher();
        var config = GetConfiguration();

        // Arrange : créer un utilisateur
        var user = new User
        {
            Username = "testuser",
            PasswordHash = hasher.Hash("password123"),
            Fullname = "Test User",
            Role = "USER"
        };
        await repo.AddAsync(user);

        var controller = new LoginController(repo, hasher, config);

        var model = new LoginModel
        {
            Username = "testuser",
            Password = "password123"
        };

        // Act
        var result = await controller.Login(model);

        // Assert
        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Contains("token", ok.Value.ToString()); // Vérifie qu'un token est présent
    }

    [Fact]
    public async Task Login_WithWrongPassword_ShouldReturnUnauthorized()
    {
        using var context = GetDbContext();
        var repo = new UserRepository(context);
        var hasher = new PasswordHasher();
        var config = GetConfiguration();

        // Arrange
        var user = new User
        {
            Username = "testuser",
            PasswordHash = hasher.Hash("password123"),
            Fullname = "Test User",
            Role = "USER"
        };
        await repo.AddAsync(user);

        var controller = new LoginController(repo, hasher, config);

        var model = new LoginModel
        {
            Username = "testuser",
            Password = "wrongpassword"
        };

        // Act
        var result = await controller.Login(model);

        // Assert
        Assert.IsType<UnauthorizedObjectResult>(result);
    }

    [Fact]
    public async Task Login_WithNonExistentUser_ShouldReturnUnauthorized()
    {
        using var context = GetDbContext();
        var repo = new UserRepository(context);
        var hasher = new PasswordHasher();
        var config = GetConfiguration();

        var controller = new LoginController(repo, hasher, config);

        var model = new LoginModel
        {
            Username = "unknown",
            Password = "password123"
        };

        var result = await controller.Login(model);

        Assert.IsType<UnauthorizedObjectResult>(result);
    }
}
