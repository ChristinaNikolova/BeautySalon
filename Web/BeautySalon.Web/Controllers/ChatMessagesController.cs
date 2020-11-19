namespace BeautySalon.Web.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using BeautySalon.Services.Data.ChatMessages;
    using Ganss.XSS;
    using Microsoft.AspNetCore.Mvc;

    //[ApiController]
    //[Route("api/[controller]")]
    public class ChatMessagesController : BaseController
    {
        private readonly IChatMessagesService chatMessagesService;

        public ChatMessagesController(IChatMessagesService chatMessagesService)
        {
            this.chatMessagesService = chatMessagesService;
        }

        public IActionResult Chat()
        {
            return this.View();
        }


        [HttpPost]
        public async Task<IActionResult> Chat(string message)
        {
            ;
            if (!this.ModelState.IsValid)
            {
                return this.NoContent();
            }

            //var sanitizer = new HtmlSanitizer();
            //var message = sanitizer.Sanitize(inputModel.Message);
            //var senderId = this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;//usermanager
            //var model = await this.chatMessagesService.Create(senderId, inputModel.UserId, message);

            //return this.Json("3");//model
            return this.View();
        }
    }
}
