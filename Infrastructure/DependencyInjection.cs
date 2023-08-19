using Application.Abstractions.Clock;
using Application.Abstractions.Data;
using Application.Abstractions.Email;
using Dapper;
using Domain.Abstraction;
using Domain.Apartments;
using Domain.Booking;
using Domain.Users;
using Infrastructure.Clock;
using Infrastructure.Data;
using Infrastructure.Email;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IDateTimeProvider, DateTimeProvider>();
        services.AddTransient<IEMailService, EmailService>();

        var connectionString = configuration.GetConnectionString("Database") ?? throw new ArgumentNullException(nameof(configuration));

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IApartmentRepository, ApartmentRepository>();
        services.AddScoped<IBookingRepository, BookingRepository>();
        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());

        services.AddSingleton<ISqlConnectionFactory>(
            _ => new SqlConnectionFactory(connectionString));

        SqlMapper.AddTypeHandler(new DapperDateOnlyTypeHandler());

        return services;
    }
}
