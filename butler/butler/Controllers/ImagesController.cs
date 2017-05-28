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
        private readonly ILogger<ImagesController> _logger;
        private readonly IDetectorService _detectorService;
        private readonly IImageService _imageService;
        private readonly IFileService _fileService;
        private readonly IIdService _idService;
        private readonly IImageTaskStatusService _imageTaskStatusService;

        public ImagesController(
            ILogger<ImagesController> logger,
            IDetectorService detectorService,
            IImageService imageService,
            IFileService fileService,
            IIdService idService,
            IImageTaskStatusService taskStatusService
            )
        {
            this._logger = logger;
            this._detectorService = detectorService;
            this._imageService = imageService;
            this._fileService = fileService;
            this._idService = idService;
            this._imageTaskStatusService = taskStatusService;
        }

        // GET: api/images
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "valuei", "valueii" };
        }

        [HttpGet("{id}/detector")]
        public IActionResult GetDetector(string id)
        {
            ImageTask imageTask = new ImageTask();
            imageTask.JobId = id;

            string detectorImagePath = this._fileService.GetDetectorImagePathWithExt(imageTask);

            if (!System.IO.File.Exists(detectorImagePath))
            {
                return NotFound("Detected image doesn't exist");
            }

            var detectorImage = System.IO.File.OpenRead(detectorImagePath);
            return File(detectorImage, "image/png");            
        }

        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            ImageTask imageTask = new ImageTask();
            imageTask.JobId = id;
            
            string mergedImagePath = this._fileService.GetMergedImagePathWithExt(imageTask);

            if (!System.IO.File.Exists(mergedImagePath))
            {
                return NotFound("Merged image doesn't exist");
            }

            var mergedImage = System.IO.File.OpenRead(mergedImagePath);
            return File(mergedImage, "image/png");            
        }

        [HttpGet("{id}/status")]
        public IEnumerable<ImageTaskStatus> GetStatus(string id)
        {
            ImageTask imageTask = new ImageTask();
            imageTask.JobId = id;

            List<ImageTaskStatus> taskStatuses = this._imageTaskStatusService.ReadLog(imageTask);
            
            return taskStatuses;
        }

        [HttpPost]
        public IActionResult Post()
        {
            IFormFileCollection files = HttpContext.Request.Form.Files;
            _logger.LogInformation("Length of files: " + files.Count);
            _logger.LogInformation("Length of keys: " + HttpContext.Request.Form.Keys.Count);
            _logger.LogInformation("Keys: " + HttpContext.Request.Form.Keys);
            IFormFile file = files.ToList().FirstOrDefault();

            if (file == null)
            {
                _logger.LogError("File is null.");
                return NotFound("File is null.");
            }
            else if (file.Length <= 0)
            {
                _logger.LogError("No files in request.");
                return NotFound("No files in request.");
            }
            else if (!this._imageService.FileTypeSupported(file))
            {
                _logger.LogError("File type is not supported.");
                return NotFound("File type is not supported.");
            }

            ImageTask imageTask = new ImageTask();
            imageTask.JobId = this._idService.GenerateId();
            imageTask.OriginalExtension = Path.GetExtension(file.FileName);
            imageTask.Status = ImageTaskStatusCode.ImageUploaded;

            string workingDir = this._fileService.GetWorkingDir(imageTask);
            string detectorDir = this._fileService.GetDetectorDir(imageTask);
            string artistDir = this._fileService.GetArtistDir(imageTask);

            this._fileService.CreateDir(workingDir);
            this._fileService.CreateDir(detectorDir);
            this._fileService.CreateDir(artistDir);

            this._imageTaskStatusService.AddToLog(
                imageTask,
                ImageTaskStatus.ImageUploaded()
            );

            string originalImagePath = this._fileService.GetOriginalImagePath(imageTask);

            using (var fileStream = new FileStream(originalImagePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
                this._detectorService.AddToQueue(imageTask);
            }
            
            return Ok(imageTask);
        }

        // PUT api/images/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
            throw new NotImplementedException();
        }

        // DELETE api/images/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

    }
}
