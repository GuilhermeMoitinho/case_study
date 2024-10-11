namespace CaseLocaliza.Notifications.Abstractions;

public interface INotifier
{
    bool HasNotification();
    ICollection<Notification> GetNotification();
    void Add(Notification notificacao);
}
