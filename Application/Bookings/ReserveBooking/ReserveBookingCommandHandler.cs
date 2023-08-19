using Application.Abstractions.Clock;
using Application.Abstractions.Messaging;
using Application.Exceptions;
using Domain.Abstraction;
using Domain.Apartments;
using Domain.Booking;
using Domain.Users;
using System.Data;

namespace Application.Bookings.ReserveBooking;

internal sealed class ReserveBookingCommandHandler : ICommandHandler<ReserveBookingCommand, Guid>
{
    private readonly IUserRepository _userRepository;
    private readonly IApartmentRepository _apartmentRepository;
    private readonly IBookingRepository _bookingRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly PricingService _pricingService;
    private readonly IDateTimeProvider _dateTImeProvider;

    public ReserveBookingCommandHandler(
        IUserRepository userRepository,
        IApartmentRepository apartmentRepository,
        IBookingRepository bookingRepository,
        IUnitOfWork unitOfWork,
        PricingService pricingService,
        IDateTimeProvider dateTImeProvider)
    {
        _userRepository = userRepository;
        _apartmentRepository = apartmentRepository;
        _bookingRepository = bookingRepository;
        _unitOfWork = unitOfWork;
        _pricingService = pricingService;
        _dateTImeProvider = dateTImeProvider;
    }

    public async Task<Result<Guid>> Handle(ReserveBookingCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId);

        if (user is null) return Result.Failure<Guid>(UserErrors.NotFound);

        var apartment = await _apartmentRepository.GetByIdAsync(request.ApartmentId);

        if (apartment is null) return Result.Failure<Guid>(ApartmentErrors.NotFound);

        var duration = DateRange.Create(request.StartDate, request.EndDate);

        if (await _bookingRepository.IsOverlappingAsync(apartment, duration, cancellationToken)) return Result.Failure<Guid>(BookingErrors.Overlap);

        try
        {
            var booking = Booking.Reserve(apartment, user.Id, duration, utcNow: _dateTImeProvider.UtcNow, _pricingService);

            _bookingRepository.Add(booking);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return booking.Id;
        }
        catch (ConcurrencyException)
        {
            return Result.Failure<Guid>(BookingErrors.Overlap);
        }
    }
}
