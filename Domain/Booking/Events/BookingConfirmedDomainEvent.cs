using Domain.Abstraction;

namespace Domain.Booking.Events;

public sealed record BookingCompletedDomainEvent(Guid bookingId) : IDomainEvent
{

}
