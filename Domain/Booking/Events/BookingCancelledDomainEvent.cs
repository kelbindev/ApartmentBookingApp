using Domain.Abstraction;

namespace Domain.Booking.Events;

public sealed record BookingCancelledDomainEvent(Guid bookingId) : IDomainEvent
{

}
