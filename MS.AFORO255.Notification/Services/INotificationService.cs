using MS.AFORO255.Notification.DTOs;
using MS.AFORO255.Notification.Models;
using System.Collections.Generic;

namespace MS.AFORO255.Notification.Services
{
    public interface INotificationService
    {
        IEnumerable<SendMail> GetAll();

        bool Add(SendMail sendMail);
    }
}
