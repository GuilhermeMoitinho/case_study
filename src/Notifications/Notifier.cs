using CaseLocaliza.Notifications.Abstractions;

namespace CaseLocaliza.Notifications;

public class Notifier : INotifier
{
    private ICollection<Notification> _notifications;

    public Notifier()
    {
        _notifications = new List<Notification>();
    }

    public void Add(Notification notificacao)
    {
         _notifications.Add(notificacao);
    }

    public ICollection<Notification> GetNotification()
    {
        return _notifications;
    }

    public bool HasNotification()
    {
        return _notifications.Any();
    }
}
