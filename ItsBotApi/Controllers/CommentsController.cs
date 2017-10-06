using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ItsBot;

namespace ItsBotApi.Controllers
{
    [Route("api/comments")]
    public class CommentsController : Controller
    {
        private IBot Bot { get; }

        public CommentsController(IBot bot)
        {
            Bot = bot;
        }

        // GET api/comments/new

        [HttpGet]
        [Route("new")]
        public IEnumerable<string> NewComments()
        {
            return new string[] { "Test", "data", "here" };
        }

    }
}
