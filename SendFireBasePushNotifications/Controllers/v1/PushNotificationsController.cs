using Application.Features.PushNotification.Commands;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SendFireBasePushNotifications.Controllers.v1
{
   [ApiVersion("1.0")]
    public class PushNotificationsController : BaseController
    {
        [Produces("application/json")]
        [HttpPost("send-notification")]
        public async Task<IActionResult> SendNotification([FromBody] SendToParameter parameter)
        {
            return Ok(await Mediator.Send(new SendPushCommand(parameter)));
        }
    }
}
