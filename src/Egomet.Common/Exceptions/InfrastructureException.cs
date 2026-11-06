
using System;

namespace Egomet.Common.Exceptions;

public class InfrastructureException(string message) : Exception(message)
{
}
