using Domain.Abstraction;

namespace Domain.Booking.Events;

public sealed record BookingReservedDomainEvent(Guid bookingId) : IDomainEvent
{

}
