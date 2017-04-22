using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using butler.Models;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.IO;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace butler.Controllers
{
    [Route("api/[controller]")]
    public class ImagesController : Controller
    {
        private readonly static string UPLOAD_DIRECTORY = "uploads/";
        private readonly ILogger _logger;

        public ImagesController(ILogger<ImagesController> logger)
        {
            _logger = logger;
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

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        /*[HttpPost]
        public string Post([FromBody]IFormFile image)
        {
            return "hallo";
        }*/

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
            else if(file.Length <= 0)
            {
                _logger.LogError("File length is zero.");
                return StatusCode(StatusCodes.Status400BadRequest);
            }

            string filePath = UPLOAD_DIRECTORY;
            filePath += string.Format("{0:yyyy-MM-dd_hh-mm-ss.fff}", DateTime.Now);
            filePath += Path.GetExtension(file.FileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
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
