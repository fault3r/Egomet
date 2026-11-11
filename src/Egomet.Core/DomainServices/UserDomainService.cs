
using System;
using Egomet.Core.Aggregates.UserAggregate;
using Egomet.Core.Common;
using Egomet.Core.Interfaces;
using Egomet.Core.Repositories;
using Egomet.Core.Security;
using Egomet.Core.ValueObjects;

namespace Egomet.Core.DomainServices;

public sealed class UserDomainService : IUserDomainService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public UserDomainService(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher)
    {
        ArgumentNullException.ThrowIfNull(userRepository);
        ArgumentNullException.ThrowIfNull(passwordHasher);

        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<Result> VerifyAvailabilityAsync(
        EmailValueObject email,
        CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(email);

        var user = await _userRepository.GetByEmailAsync(email, ct);

        return user is null
            ? Result.Success()
            : Result.Failure("Email address is already registered.");
    }

    public async Task<Result<UserAggregate>> VerifyCredentialsAsync(
        EmailValueObject email,
        string password,
        CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(email);
        ArgumentException.ThrowIfNullOrWhiteSpace(password);

        var user = await _userRepository.GetByEmailAsync(email, ct);

        if (user is null)
            return Result<UserAggregate>.Failure("Invalid email or password.");

        var isPasswordValid = _passwordHasher.Verify(password, user.PasswordHash);

        if (!isPasswordValid)
            return Result<UserAggregate>.Failure("Invalid email or password.");

        return Result<UserAggregate>.Success(user);
    }
}