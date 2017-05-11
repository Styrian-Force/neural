using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.IO;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading;

using Butler.Models;
using Butler.Services;
using Butler.Interfaces;
using Butler.Config;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Butler.Controllers
{


    [Route("api/[controller]")]
    public class ImagesController : Controller
    {
        private readonly ILogger _logger;
        private readonly IDetectorService _detectorService;
        private readonly IArtistService _artistService;
        private readonly IImageService _imageService;
        private readonly IFileService _fileService;
        private readonly IIdService _idService;


        public ImagesController(
            ILogger<ImagesController> logger,
            IDetectorService detectorService,
            IArtistService artistService,
            IImageService imageService,
            IFileService fileService,
            IIdService idService
            )
        {
            this._logger = logger;
            this._detectorService = detectorService;
            this._imageService = imageService;
            this._artistService = artistService;
            this._fileService = fileService;
            this._idService = idService;
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "valuei", "valueii" };
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            ImageTask imageTask = new ImageTask();
            imageTask.JobId = id;
            // TODO: Change to artist
            string detectorImagePath = this._fileService.GetDetectorImagePathWithExt(imageTask);

            if (!System.IO.File.Exists(detectorImagePath))
            {
                return StatusCode(404);
            }

            var detectorImage = System.IO.File.OpenRead(detectorImagePath);
            return File(detectorImage, "image/png");
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
                _logger.LogError("No files in request.");
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            else if (!this._imageService.FileTypeSupported(file))
            {
                _logger.LogError("File type is not supported.");
                return StatusCode(StatusCodes.Status400BadRequest);
            }

            ImageTask imageTask = new ImageTask();
            imageTask.JobId = this._idService.GenerateId();
            imageTask.OriginalExtension = Path.GetExtension(file.FileName);

            string workingDir = this._fileService.GetWorkingDir(imageTask);
            string detectorDir = this._fileService.GetDetectorDir(imageTask);
            string artistDir = this._fileService.GetArtistDir(imageTask);

            this._fileService.CreateDir(workingDir);
            this._fileService.CreateDir(detectorDir);
            this._fileService.CreateDir(artistDir);            

            string originalImagePath = this._fileService.GetOriginalImagePath(imageTask);

            using (var fileStream = new FileStream(originalImagePath, FileMode.Create))
            {
                file.CopyTo(fileStream);                
                imageTask.task = new Task(() => { });

                this._detectorService.AddToQueue(imageTask);
                imageTask.task.Wait();
                Console.WriteLine("DETECTOR_END");

                imageTask.task = new Task(() => { });

                this._artistService.AddToQueue(imageTask);
                imageTask.task.Wait();
                Console.WriteLine("ARTIST_END");

                this._imageService.MergeImages(imageTask);
                Console.WriteLine("IMAGE_SERVICE_END");

                string detectorImagePath = this._fileService.GetDetectorImagePathWithExt(imageTask);
                var detectorImage = System.IO.File.OpenRead(detectorImagePath);
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

    }
}
