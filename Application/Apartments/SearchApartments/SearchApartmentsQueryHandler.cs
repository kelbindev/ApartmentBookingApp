using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Dapper;
using Domain.Abstraction;
using System.Net.WebSockets;

namespace Application.Apartments.SearchApartments;

internal sealed class SearchApartmentsQueryHandler : IQueryHandler<SearchApartmentsQuery, IReadOnlyList<ApartmentResponse>>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public SearchApartmentsQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<Result<IReadOnlyList<ApartmentResponse>>> Handle(SearchApartmentsQuery request, CancellationToken cancellationToken)
    {
        if (request.StartDate >= request.EndDate) return new List<ApartmentResponse>();

        using var connection = _sqlConnectionFactory.CreateConnection();

        const string sql = "";

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
            }, splitOn:"Country");

        return apartments.ToList();
    }
}

