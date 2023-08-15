using Application.Abstractions.Email;
using Domain.Booking;
using Domain.Booking.Events;
using Domain.Users;
using MediatR;

namespace Application.Bookings.ReserveBooking;

internal sealed class BookingReservedDomainEventHandler : INotificationHandler<BookingReservedDomainEvent>
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IUserRepository _userRepository;
    private readonly IEMailService _emailService;

    public BookingReservedDomainEventHandler(
        IBookingRepository bookingRepository, 
        IUserRepository userRepository, 
        IEMailService emailService)
    {
        _bookingRepository = bookingRepository;
        _userRepository = userRepository;
        _emailService = emailService;
    }

    public async Task Handle(BookingReservedDomainEvent notification, CancellationToken cancellationToken)
    {
        var booking = await _bookingRepository.GetByIdAsync(notification.bookingId, cancellationToken);

        if (booking == null)
        {
            return;
        }

        var user = await _userRepository.GetByIdAsync(booking.UserId, cancellationToken);

        if (user == null)
        {
            return;
        }

        await _emailService.SendAsync(user.Email,"Booking Reserved!","You have 10 minutes to confirm this booking");
    }
}
