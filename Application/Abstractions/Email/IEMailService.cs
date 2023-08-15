namespace Application.Abstractions.Email;

public interface IEMailService
{
    Task SendAsync(Domain.Users.Email recipient, string subject, string body);

}
