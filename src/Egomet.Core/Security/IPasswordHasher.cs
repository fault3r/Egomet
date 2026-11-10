
using System;

namespace Egomet.Core.Security;

public interface IPasswordHasher
{
    string Compute(string raw);
    bool Verify(string raw, string hash);
}