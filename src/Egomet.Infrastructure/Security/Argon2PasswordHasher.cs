
using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using Egomet.Core.Security;
using Konscious.Security.Cryptography;

namespace Egomet.Infrastructure.Security;

public sealed class Argon2PasswordHasher
    : IPasswordHasher
{
    private const int MemorySize = 65536;
    private const int Iterations = 3;
    private const int DegreeOfParallelism = 1;

    private const int SaltSize = 16;
    private const int HashSize = 32;

    private const int Version = 19;

    public string Compute(string raw)
    {
        ArgumentNullException.ThrowIfNull(raw);

        byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);

        byte[] hash = HashPassword(
            raw,
            salt,
            MemorySize,
            Iterations,
            DegreeOfParallelism
        );

        return $"$argon2id$v={Version}$m={MemorySize},t={Iterations},p={DegreeOfParallelism}${Convert.ToBase64String(salt)}${Convert.ToBase64String(hash)}";
    }

    public bool Verify(string raw, string hash)
    {
        ArgumentNullException.ThrowIfNull(raw);

        if (string.IsNullOrWhiteSpace(hash) || hash.Length > 512)
            return false;

        string[] parts = hash.Split('$');

        if (parts.Length != 6 ||
            parts[0] != "" ||
            parts[1] != "argon2id" ||
            parts[2] != $"v={Version}")
        {
            return false;
        }

        try
        {
            string[] parameters = parts[3].Split(',');

            if (parameters.Length != 3 ||
                !TryParseParameter(parameters[0], "m", out int memory) ||
                !TryParseParameter(parameters[1], "t", out int iterations) ||
                !TryParseParameter(parameters[2], "p", out int parallelism))
            {
                return false;
            }

            if (memory < 19456 || memory > 262144 ||
                iterations < 2 || iterations > 10 ||
                parallelism < 1 || parallelism > 4)
            {
                return false;
            }

            byte[] salt = Convert.FromBase64String(parts[4]);
            byte[] expectedHash = Convert.FromBase64String(parts[5]);

            if (salt.Length < SaltSize || salt.Length > 64 ||
                expectedHash.Length < 16 || expectedHash.Length > 64)
            {
                return false;
            }

            byte[] actualHash = HashPassword(
                raw,
                salt,
                memory,
                iterations,
                parallelism,
                expectedHash.Length
            );

            return CryptographicOperations.FixedTimeEquals(
                actualHash,
                expectedHash
            );
        }
        catch (FormatException)
        {
            return false;
        }
    }

    private static byte[] HashPassword(
        string raw,
        byte[] salt,
        int memory,
        int iterations,
        int parallelism,
        int hashSize = HashSize)
    {
        byte[] passwordBytes = Encoding.UTF8.GetBytes(raw);

        using var argon2 = new Argon2id(passwordBytes)
        {
            Salt = salt,
            MemorySize = memory,
            Iterations = iterations,
            DegreeOfParallelism = parallelism
        };

        return argon2.GetBytes(hashSize);
    }

    private static bool TryParseParameter(
        string parameter,
        string name,
        out int value)
    {
        value = 0;

        if (!parameter.StartsWith($"{name}=", StringComparison.Ordinal))
            return false;

        return int.TryParse(
            parameter.AsSpan(name.Length + 1),
            NumberStyles.None,
            CultureInfo.InvariantCulture,
            out value
        );
    }

    public string DummyHash { get; }

    public Argon2PasswordHasher()
    {
        DummyHash = Compute("Egomet.Infrastructure.Security.Dummy.Hash!");
    }
}
