using Domain.Abstraction;

namespace Domain.Booking.Events;

public sealed record BookingConfirmedDomainEvent(Guid bookingId) : IDomainEvent
{

}
