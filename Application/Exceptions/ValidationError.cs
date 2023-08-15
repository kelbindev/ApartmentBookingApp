namespace Application.Exceptions;

public sealed record ValidationError(string propertyName, string errorMessage);
