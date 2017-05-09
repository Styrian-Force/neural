using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Butler.Models;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.IO;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using Butler.Services;
using Butler.Interfaces;
using System.Threading;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Butler.Controllers
{


    [Route("api/[controller]")]
    public class ImagesController : Controller
    {
        private readonly static string UPLOAD_DIRECTORY = "/home/administrator/dev/neural/database/";


        private readonly ILogger _logger;
        private readonly IDetectorService _detectorService;
        private readonly IIdService _idService;

        public ImagesController(
            ILogger<ImagesController> logger,
            IDetectorService detectorService,
            IIdService idService
            )
        {
            _logger = logger;
            this._detectorService = detectorService;
            this._idService = idService;

            if (!Directory.Exists(UPLOAD_DIRECTORY))
            {
                Directory.CreateDirectory(UPLOAD_DIRECTORY);
            }
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "valuei", "valueii" };
        }

        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpPost]
        public StatusCodeResult Post()
        {
            IFormFileCollection files = HttpContext.Request.Form.Files;

            IFormFile file = files.ToList().FirstOrDefault();

            if (file == null)
            {
                _logger.LogError("File is null.");
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            else if (file.Length <= 0)
            {
                _logger.LogError("File length is zero.");
                return StatusCode(StatusCodes.Status400BadRequest);
            }

            string workingDir = UPLOAD_DIRECTORY;
            workingDir += string.Format("{0:yyyy-MM-dd_HH-mm-ss.fff}/", DateTime.Now);
            string subDir = workingDir + "detected_objects/";

            if (!Directory.Exists(workingDir))
            {
                Directory.CreateDirectory(workingDir);
            }
            if (!Directory.Exists(subDir))
            {
                Directory.CreateDirectory(subDir);
            }

            string inputFilePath = workingDir + "butler_output" + Path.GetExtension(file.FileName);

            using (var fileStream = new FileStream(inputFilePath, FileMode.Create))
            {
                file.CopyTo(fileStream);

                ImageTask imageTask = new ImageTask();
                imageTask.JobId = this._idService.GenerateId();
                imageTask.WorkingDir = workingDir;
                imageTask.SubDir = subDir;
                imageTask.InputFilePath = inputFilePath;
                imageTask.task = new Task(() => {});

                this._detectorService.AddToQueue(imageTask);
                imageTask.task.Wait();              

                Console.WriteLine("KONEC IZVEDBE");
            }

            return StatusCode(200);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
