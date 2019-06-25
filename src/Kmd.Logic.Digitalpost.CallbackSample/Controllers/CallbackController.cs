using System;
using System.IO;
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
            var url = $"{Request.Scheme}://{Request.Host.Value}{Request.Path.Value}";
            return View((object)url);
        }

        [HttpPut("")]
        public IActionResult Put(CallbackModel model)
        {
            logger.LogInformation("Got response from citizen: {model}", JsonConvert.SerializeObject(model));
            var saver = new FileSaver("Download");
            saver.SaveFile($"content.{model.ContentType}", model.Content);
            model.Attachments?.ForEach(x => saver.SaveFile(x.Name, x.Content));
            return Ok();
        }
    }

    public class FileSaver
    {
        private readonly string _directory;

        public FileSaver(string directory)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            _directory = directory;
        }

        public void SaveFile(string filename, string contentInBase64)
        {
            SaveFile(filename, Convert.FromBase64String(contentInBase64));
        }

        public void SaveFile(string filename, byte[] bytes)
        {
            var filePath = Path.Combine(_directory, filename);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            
            File.WriteAllBytes(filePath, bytes);
        }

    }
}
