using Domain.Abstraction;

namespace Domain.Booking.Events;

public sealed record BookingRejectedDomainEvent(Guid bookingId) : IDomainEvent
{

}
