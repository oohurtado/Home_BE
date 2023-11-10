using Home.Source.Services.Message;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;

namespace Home.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MiscController : ControllerBase
    {

        [HttpGet(template: "getEmail")]
        public ActionResult GetEmail([FromServices] IEnumerable<IMessageService> messageServices)
        {
            var messageService = messageServices.OfType<EmailService>().First();
            messageService.SendMessage();

            return Ok();
        }

        [HttpGet(template: "getSMS")]
        public ActionResult GetSMS([FromServices] IEnumerable<IMessageService> messageServices)
        {
            var messageService = messageServices.First(p => p.MessageServiceType == MessageServiceType.SMS);
            messageService.SendMessage();

            return Ok();
        }
    }
}
