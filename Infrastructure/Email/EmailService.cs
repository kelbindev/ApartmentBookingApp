using Application.Abstractions.Email;

namespace Infrastructure.Email;

internal sealed class EmailService : IEMailService
{
    public Task SendAsync(Domain.Users.Email recipient, string subject, string body)
    {
        return Task.CompletedTask;
    }
}
