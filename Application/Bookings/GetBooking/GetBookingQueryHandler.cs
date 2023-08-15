using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Dapper;
using Domain.Abstraction;
using System.Net.WebSockets;

namespace Application.Bookings.GetBooking;

internal sealed class GetBookingQueryHandler : IQueryHandler<GetBookingQuery, BookingResponse>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public GetBookingQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<Result<BookingResponse>> Handle(GetBookingQuery request, CancellationToken cancellationToken)
    {
        using var connection = _sqlConnectionFactory.CreateConnection();

        const string sql = "";

        var booking = await connection.QueryFirstOrDefaultAsync<BookingResponse>(sql, new
        {
            request.BookingId
        });

        return booking;
    }
}
