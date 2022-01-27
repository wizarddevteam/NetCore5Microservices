using Microsoft.AspNetCore.Mvc;
using MS.AFORO255.Notification.Services;
using System.Linq;
using System.Threading.Tasks;

namespace MS.AFORO255.Notification.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_notificationService.GetAll());
        }
    }
}
