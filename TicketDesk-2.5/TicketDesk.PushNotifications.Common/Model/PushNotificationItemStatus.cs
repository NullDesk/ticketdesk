namespace TicketDesk.PushNotifications.Common.Model
{
    public enum PushNotificationItemStatus
    {
        Scheduled,
        Sending,
        Sent,
        Retrying,
        Failed,
        Canceled,
        Disabled

    }
}
