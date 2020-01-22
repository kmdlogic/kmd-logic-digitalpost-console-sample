using System;
using Kmd.Logic.Digitalpost.CallbackSample.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Kmd.Logic.Digitalpost.CallbackSample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CallbackController : Controller
    {
        private ILogger logger;

        public CallbackController(ILogger logger)
        {
            this.logger = logger;
        }

        [HttpGet]
        [HttpHead]
        public IActionResult Get()
        {
            var url = $"{this.Request.Scheme}://{this.Request.Host.Value}{this.Request.Path.Value}";
            return this.View((object)url);
        }

        [HttpPut("")]
        public IActionResult Put(CallbackModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            this.logger.LogInformation("Got response from citizen: {model}", JsonConvert.SerializeObject(model));
            var saver = new FileSaver("Download");
            saver.SaveFile($"content.{model.ContentType}", model.Content);
            model.Attachments?.ForEach(x => saver.SaveFile(x.Name, x.Content));
            return this.Ok();
        }
    }
}
