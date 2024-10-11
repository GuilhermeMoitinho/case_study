using FluentValidation.Results;

namespace CaseLocaliza.Notifications;

public class Notification : ValidationFailure
{
    public string Message { get; private set; }

    public Notification(string message)
    {
        Message = message;
    }
}