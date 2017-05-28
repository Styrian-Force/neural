using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Butler.Models;
using Butler.Interfaces;
using Microsoft.Extensions.Logging;

namespace Butler.Controllers
{
    [Route("api/[controller]")]
    public class ImageTasksController : Controller
    {
        private readonly ILogger<ImageTasksController> _logger;
        private readonly IImageTaskStatusService _imageTaskStatusService;

        public ImageTasksController(
                    ILogger<ImageTasksController> logger,
                    IImageTaskStatusService taskStatusService
                    )
        {
            this._logger = logger;
            this._imageTaskStatusService = taskStatusService;
        }

        // GET api/imageTasks
        [HttpGet]
        public IEnumerable<Value> Get()
        {
            throw new NotImplementedException();
        }

        // GET api/imageTasks/5
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            ImageTask imageTask = this._imageTaskStatusService.DeserializeImageTask(id);
            if (imageTask == null)
            {
                return NotFound("ImageTask doesn't exist");
            }
            return Ok(imageTask);
        }

        // POST api/imageTasks
        [HttpPost]
        public void Post([FromBody]string value)
        {
            throw new NotImplementedException();
        }

        // PUT api/imageTasks/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
            throw new NotImplementedException();
        }

        // DELETE api/imageTasks/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
