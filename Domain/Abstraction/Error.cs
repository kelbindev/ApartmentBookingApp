namespace Domain.Abstraction;

public record Error(string Code, string Name)
{
    public static Error None = new(String.Empty, string.Empty);
    public static Error NullValue = new("Error.NullValue", "Null value was provided");
}
