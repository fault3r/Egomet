
using System;

namespace Egomet.Infrastructure.Exceptions;

public class InfrastructureException(string message) : Exception(message) 
{ }