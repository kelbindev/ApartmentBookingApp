using Domain.Abstraction;

namespace Domain.Booking;

public static class BookingErrors
{
    public static Error NotFound = new("Booking.Found","the booking with specific identifier was not found");
    public static Error Overlap = new("Booking.Overlap","the booking is overlapping with existing one");
    public static Error NotReserverd = new("Booking.NotReserverd", "the booking is not pending");
    public static Error NotConfirmed = new("Booking.NotConfirmed", "the booking is not confrmed");
    public static Error AlreadyStarted = new("Booking.AlreadyStarted", "the booking has already started");
}