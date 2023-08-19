using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Dapper;
using Domain.Abstraction;
using Domain.Booking;
using System.Net.WebSockets;

namespace Application.Apartments.SearchApartments;

internal sealed class SearchApartmentsQueryHandler : IQueryHandler<SearchApartmentsQuery, IReadOnlyList<ApartmentResponse>>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    private static readonly int[] ActiveBookingStatuses =
    {
        (int)BookingStatus.Reserved,
        (int)BookingStatus.Confirmed,
        (int)BookingStatus.Completed
    };

    public SearchApartmentsQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<Result<IReadOnlyList<ApartmentResponse>>> Handle(SearchApartmentsQuery request, CancellationToken cancellationToken)
    {
        if (request.StartDate >= request.EndDate) return new List<ApartmentResponse>();

        using var connection = _sqlConnectionFactory.CreateConnection();

        const string sql = @"
            SELECT
                a.Id AS Id,
                a.Name AS Name,
                a.Description AS Description,
                a.PriceAmount AS Price,
                a.PriceCurrency AS Currency,
                a.AddressCountry AS Country,
                a.AddressState AS State,
                a.AddressZipCode AS ZipCode,
                a.AddressCity AS City,
                a.AddressStreet AS Street
            FROM Apartment AS a
            WHERE NOT EXISTS
            (
                SELECT 1
                FROM Booking AS b
                WHERE
                    b.ApartmentId = a.Id AND
                    b.DurationStart <= @EndDate AND
                    b.DurationEnd >= @StartDate AND
                    b.Status = ANY(@ActiveBookingStatuses)
            )";

        var apartments = await connection.QueryAsync<ApartmentResponse, AddressResponse, ApartmentResponse>(
            sql,
            (apartment, address) =>
            {
                apartment.Address = address;

                return apartment;
            },
            new
            {
                request.StartDate,
                request.EndDate,
                ActiveBookingStatuses
            }, splitOn: "Country");

        return apartments.ToList();
    }
}

