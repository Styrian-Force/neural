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
        private readonly IArtistService _artistService;
        private readonly IImageService _imageService;
        private readonly IIdService _idService;


        public ImagesController(
            ILogger<ImagesController> logger,
            IDetectorService detectorService,
            IArtistService artistService,
            IImageService imageService,
            IIdService idService
            )
        {
            this._logger = logger;
            this._detectorService = detectorService;
            this._imageService = imageService;
            this._artistService = artistService;
            this._idService = idService;

            CreateDir(UPLOAD_DIRECTORY);
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
        public async Task<IActionResult> Post()
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

            string jobId = this._idService.GenerateId();
            string workingDir = UPLOAD_DIRECTORY + jobId + "/";
            string detectorDir = workingDir + "detector/";
            string artistDir = workingDir + "artist/";

            CreateDir(workingDir);
            CreateDir(detectorDir);
            CreateDir(artistDir);

            string originalImagePath = workingDir + "butler" + Path.GetExtension(file.FileName);

            using (var fileStream = new FileStream(originalImagePath, FileMode.Create))
            {
                file.CopyTo(fileStream);

                ImageTask imageTask = new ImageTask();
                imageTask.JobId = jobId;
                imageTask.WorkingDir = workingDir;
                imageTask.DetectorDir = detectorDir;
                imageTask.ArtistDir = artistDir;
                imageTask.OriginalImagePath = originalImagePath;
                imageTask.task = new Task(() => { });

                this._detectorService.AddToQueue(imageTask);
                imageTask.task.Wait();
                Console.WriteLine("DETECTOR_END");

                imageTask.task = new Task(() => { });

                /*this._artistService.AddToQueue(imageTask);
                imageTask.task.Wait();
                Console.WriteLine("ARTIST_END");

                this._imageService.MergeImages(imageTask);
                Console.WriteLine("IMAGE_SERVICE_END");*/

                var detectorImage = System.IO.File.OpenRead(imageTask.WorkingDir + "detector_output.png");
                return File(detectorImage, "image/png");
            }       
            //return StatusCode(200);
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

        private void CreateDir(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
    }
}
