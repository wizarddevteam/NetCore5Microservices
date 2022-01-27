using Aforo255.Cross.Event.Src.Bus;
using MS.AFORO255.Notification.Messages.Events;
using MS.AFORO255.Notification.Models;
using MS.AFORO255.Notification.Services;
using System.Threading.Tasks;

namespace MS.AFORO255.Notification.Messages.EventHandlers
{
    public class NotificationEventHandler : IEventHandler<NotificationCreatedEvent>
    {
        private readonly INotificationService _notificationService;

        public NotificationEventHandler(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public Task Handle(NotificationCreatedEvent @event)
        {
            _notificationService.Add(new SendMail()
            {
                SendMailId = @event.IdTransaction,
                Type = @event.Type,
                AccountId = @event.AccountId,
                Message = @event.MessageBody,
                Address = @event.Address,
                SendDate = @event.Timestamp.ToLongDateString()
            });
            return Task.CompletedTask;
        }
    }
}
