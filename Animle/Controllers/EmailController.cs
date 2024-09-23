using Animle.interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Animle.Controllers
{
    [Route("email")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        [EnableRateLimiting("fixed")]
        [Route("form")]
        [HttpPost]
        public IActionResult SendEmail(EmailDto email)
        {
            SimpleResponse simpleResponse = new SimpleResponse();

            var emailSent = EmailService.SendEmail(email);
            if (emailSent == false)
            {
                simpleResponse.Response = "There was a problem send the email. Try it later please";
                return BadRequest(simpleResponse);
            }
            simpleResponse.Response = "ok";
            return Ok(simpleResponse);

        }
    }
}
