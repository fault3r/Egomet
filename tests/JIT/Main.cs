
using System;
using Egomet.Infrastructure.Security;

namespace JIT;

public class JITTest
{
    [Fact]
    public void MainTest()
    {
        var hasher = new Argon2PasswordHasher();
        Console.WriteLine(hasher.DummyHash);
        Console.WriteLine(hasher.DummyHash);
        Console.WriteLine(hasher.DummyHash);
        Console.WriteLine(hasher.DummyHash);
        Console.WriteLine(hasher.DummyHash);
        Assert.True(true);
    }
}
