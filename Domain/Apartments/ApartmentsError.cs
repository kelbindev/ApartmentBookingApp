using Domain.Abstraction;

namespace Domain.Apartments;

public static class ApartmentsError
{
    public static Error NotFound = new("Apartment.Found", "the apartment with specific identifier was not found");
}
