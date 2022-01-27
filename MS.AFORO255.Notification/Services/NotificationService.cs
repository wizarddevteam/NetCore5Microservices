using MS.AFORO255.Notification.Models;
using MS.AFORO255.Notification.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace MS.AFORO255.Notification.Services
{
    public class NotificationService : INotificationService
    {
        private readonly ContextDatabase _contextDatabase;

        public NotificationService(ContextDatabase contextDatabase)
        {
            _contextDatabase = contextDatabase;
        }

        public bool Add(SendMail sendMail)
        {
            _contextDatabase.SendMail.Add(sendMail);
            _contextDatabase.SaveChanges();
            return true;
        }

        public IEnumerable<SendMail> GetAll()
        {
            return _contextDatabase.SendMail.ToList();
        }
    }
}
