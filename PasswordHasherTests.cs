using P7CreateRestApi.Services;
using Xunit;

public class PasswordHasherTests
{
    private readonly PasswordHasher _hasher = new PasswordHasher();

    [Fact]
    public void Hash_And_Verify_Should_Work()
    {
        var password = "Test123!";
        var hash = _hasher.Hash(password);

        Assert.True(_hasher.Verify(hash, password));
        Assert.False(_hasher.Verify(hash, "WrongPassword"));
    }
}
