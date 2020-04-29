using System.ComponentModel;

namespace WebAPIService.Enums
{
    public enum MessageBusEvents {
        [Description ("user.notification.event")]
        UserNotificationEvent = 0
    }
}