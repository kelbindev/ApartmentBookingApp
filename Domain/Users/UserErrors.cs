using Domain.Abstraction;

namespace Domain.Users;

public static class UserErrors
{
    public static Error NotFound = new("User.Found", "the user with specific identifier was not found");
}
