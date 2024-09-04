using Microsoft.AspNetCore.Mvc;
using PMFWikipedia.InterfacesBL;

namespace PMFWikipedia.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationBL _notificationBL;

        public NotificationController(INotificationBL notificationBL)
        {
            _notificationBL = notificationBL;
        }
        [HttpGet]
        public async Task<IActionResult> GetNumberOfUnreadNotification(long id)
        {
            return Ok(await _notificationBL.GetUnreadNotification(id));
        }

        [HttpGet("all-notification")]
        public async Task<IActionResult> GetAllNotifications(long id)
        {
            return Ok(await _notificationBL.GetAllNotification(id));
        }
    }
}