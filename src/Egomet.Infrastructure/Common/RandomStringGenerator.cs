
using System;
using System.Security.Cryptography;

namespace Egomet.Infrastructure.Common;

public static class RandomStringGenerator
{
    public const string Characters =
        "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

    public static string GetString(int length = 8)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(length);

        var result = new char[length];

        for (int i = 0; i < length; i++)
        {
            int index = RandomNumberGenerator.GetInt32(Characters.Length);
            result[i] = Characters[index];
        }

        return new string(result);
    }
}
